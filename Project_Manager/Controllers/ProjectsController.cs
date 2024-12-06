using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Mappers;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.Services;
using Project_Manager.ViewModels;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly ProjectUserService _projectUserService;

        public ProjectsController(IProjectRepository projectRepository, IProjectUserRepository projectUserRepository,
            ProjectUserService projectUserService)
        {
            _projectRepository = projectRepository;
            _projectUserRepository = projectUserRepository;
            _projectUserService = projectUserService;
        }

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

/*        private string? GetInvitationLink(int projectId)
        {
            return Url.Action("Join", "JoinProject", new { projectId }, Request.Scheme);
        }*/


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User is not authenticated.");

            var projects = await _projectUserService.GetUserProjectsAsync(userId);
            var projectsDTO = projects.Select(t => t.ToProjectDTO()).ToList();
            return View(projectsDTO);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAndEditProjectVM createProjectVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User is not authenticated.");

            var createdProject = await _projectRepository.CreateAsync(createProjectVM.ToProject());
            await _projectUserService.AddUserToProjectAsync(createdProject.Id, userId, UserRoles.Admin);

            return RedirectToAction("Index", "ProjectTasks", new { projectId = createdProject.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int projectId)
        {
            //TODO логика поиска задач заданной команды

            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null) return NotFound("Project not found.");

            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User is not authenticated.");

            var userRole = await _projectUserRepository.GetUserRoleInProjectAsync(userId, projectId);
            if (userRole == null) return NotFound("User is not in the project.");

            var projectDetailsVM = new ProjectDetailsVM
            {
                ProjectId = projectId,
                ProjectName = project.Name,
                UserRoles = (UserRoles)userRole,
                InvitationLink = Url.Action("Join", "JoinProject", new { projectId }, Request.Scheme)
                //TODO список задач команды
            };

            return View(projectDetailsVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);

            if (project == null)
                return NotFound("Project not found");

            return View(project.ToCreateAndEditProjectVM());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateAndEditProjectVM editProjectVM)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("", "Failed to edit car");
                return View("Edit", editProjectVM);
            }

            await _projectRepository.UpdateAsync(
                new Project
                {
                    Id = id,
                    Name = editProjectVM.Name
                }
                );

            return RedirectToAction("Details", "Projects", new { projectId = id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return View("Error");
            }
            return View(project.ToProjectDTO());
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleteResponse = await _projectRepository.DeleteAsync(id);
            if (!deleteResponse)
            {
                return View("Error");
            }
            return RedirectToAction("Index", "Projects");
        }
    }
}
