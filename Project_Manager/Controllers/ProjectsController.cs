using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Helpers;
using Project_Manager.Models.Enums;
using Project_Manager.Services.Interfaces;
using Project_Manager.ViewModels;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly IProjectUserService _projectUserService;
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectRepository projectRepository, IProjectUserRepository projectUserRepository,
            IProjectUserService projectUserService, IProjectService projectService)
        {
            _projectRepository = projectRepository;
            _projectUserRepository = projectUserRepository;
            _projectUserService = projectUserService;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _projectService.GetUserProjectsAsync(User.GetUserId()));
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
                return View(createProjectVM);

            var createdProject = await _projectService.CreateProjectAsync(User.GetUserId(), createProjectVM);

            if (createdProject == null)
            {
                TempData["ErrorMessage"] = "Не удалось определить пользователя. Пожалуйста, войдите в систему.";
                return RedirectToAction("Index", "Error");
            }

            return RedirectToAction("Index", "ProjectTasks", new { projectId = createdProject.Id });
        }

        // TODO not using УДАЛИТЬ!
        [HttpGet]
        public async Task<IActionResult> Details(int projectId)
        {
            //TODO логика поиска задач заданной команды

            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null) return NotFound("Project not found.");

            var userId = User.GetUserId();
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
            var projectName = await _projectService.GetProjectName(id);

            if (projectName == null)
            {
                TempData["ErrorMessage"] = "Не удалось найти проект.";
                return RedirectToAction("Index", "Error");
            }

            return View(new CreateAndEditProjectVM(projectName));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateAndEditProjectVM editProjectVM)
        {
            if (!ModelState.IsValid)
                return View(editProjectVM);

            if (!await _projectService.UpdateProjectAsync(id, editProjectVM))
            {
                TempData["ErrorMessage"] = "Ошибка при обновлении проекта.";
                return RedirectToAction("Index", "Error");
            }

            return RedirectToAction("Index", "ProjectTasks", new { projectId = id });
        }

        //TODO сделать pop-up
        [HttpGet]
        public async Task<IActionResult> Delete(int projectId)
        {
            if (await _projectService.ExistProjectAsync(projectId))
            {
                return View(/*project.ToProjectDTO()*/);
            }
            return View("Error");
            
        }

        //TODO сделать pop-up
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int projectId)
        {
            bool deleteResponse = await _projectRepository.DeleteAsync(projectId);
            if (!deleteResponse)
            {
                return View("Error");
            }
            return RedirectToAction("Index", "Projects");
        }
    }
}
