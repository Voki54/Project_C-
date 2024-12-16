using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Helpers;
using Project_Manager.Services.Interfaces;
using Project_Manager.ViewModels;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
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

            if (createdProject != null)
                return RedirectToAction("Index", "ProjectTasks", new { projectId = createdProject.Id });

            TempData["ErrorMessage"] = "Не удалось создать проект.";
            return RedirectToAction("Index", "Error");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int projectId)
        {
            var projectName = await _projectService.GetProjectName(projectId);

            if (projectName != null)
                return View(new CreateAndEditProjectVM(projectId, projectName));

            TempData["ErrorMessage"] = "Не удалось найти проект.";
            return RedirectToAction("Index", "Error");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateAndEditProjectVM editProjectVM)
        {
            if (!ModelState.IsValid)
                return View(editProjectVM);

            if (await _projectService.UpdateProjectAsync(editProjectVM))
                return RedirectToAction("Index", "ProjectTasks", new { projectId = editProjectVM.Id });
            
            TempData["ErrorMessage"] = "Ошибка при обновлении проекта.";
            return RedirectToAction("Index", "Error");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int projectId)
        {
            if (await _projectService.DeleteProjectAsync(projectId))
                return RedirectToAction("Index", "Projects");

            TempData["ErrorMessage"] = "Ошибка при удалении проекта.";
            return RedirectToAction("Index", "Error");
        }
    }
}