using Microsoft.AspNetCore.Mvc;
using Project_Manager.Models;
using Project_Manager.ViewModels;
using Project_Manager.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Project_Manager.Services.Interfaces;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class ProjectTasksController : Controller
    {
        private readonly IProjectTasksService _taskService;
        private readonly IUserAccessService _userAccessService;

        public ProjectTasksController(IProjectTasksService taskService, IUserAccessService userAccessService)
        {
            _taskService = taskService;
            _userAccessService = userAccessService;
        }

        public async Task<IActionResult> Index(int projectId, int? categoryId, string? sortColumn, string? filterStatus, string? filterExecutor, DateTime? filterDate)
        {
            if (!await _userAccessService.IsCurrentUserExecutorOrManagerOrAdminWithProjectAccessAsync(projectId))
            {
                return NotFound("Нет доступа к проекту.");
            }

            TaskCategoryVM model;

            try
            {
                model = await _taskService.GetTaskCategoryVMAsync(projectId, categoryId, sortColumn, filterStatus, filterExecutor, filterDate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка при получении задач проекта.");
            }

            return View(model);
        }


        public async Task<IActionResult> Create(int projectId)
        {
            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithProjectAccessAsync(projectId))
            {
                return NotFound("Нет доступа к проекту.");
            }

            return await GetCreateReadTaskVM(projectId, null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectTask task, int projectId)
        {
            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithProjectAccessAsync(projectId))
            {
                return NotFound("Нет доступа к проекту.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _taskService.CreateTaskAsync(task);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Ошибка при добавлении задачи.");
                }
                return RedirectToAction("Index", new { projectId }); 
            }

            return await GetCreateReadTaskVM(projectId, null);
        }


        public async Task<IActionResult> Edit(int id, int projectId)
        {
            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithTaskAccessAsync(id))
            {
                return NotFound("Нет доступа к задаче.");
            }

            return await GetCreateReadTaskVM(projectId, id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectTask task, int projectId)
        {
            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithTaskAccessAsync(id))
            {
                return NotFound("Нет доступа к задаче.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _taskService.UpdateTaskAsync(task);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Ошибка при обновлении задачи.");
                }
                return RedirectToAction("Index", new { projectId });
            }

            return await GetCreateReadTaskVM(projectId, id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int projectId)
        {
            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithTaskAccessAsync(id))
            {
                return NotFound("Нет доступа к задаче.");
            }

            try
            {
                await _taskService.DeleteTaskAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка при удалении задачи.");
            }

            return RedirectToAction("Index", new { projectId });
        }


        public async Task<IActionResult> ViewTask(int id, int projectId)
        {
            if (!await _userAccessService.IsCurrentUserExecutorOrManagerOrAdminWithTaskAccessAsync(id))
            {
                return NotFound("Нет доступа к задаче.");
            }

            ViewTaskVM model = null;

            try
            {
                model = await _taskService.GetViewTaskVMAsync(id, projectId);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка при получении задачи.");
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddComment(int taskId, string content, int projectId)
        {
            if (!await _userAccessService.IsCurrentUserExecutorWithTaskAccessAsync(taskId))
            {
                return NotFound("Нет доступа к задаче.");
            }

            try
            {
                await _taskService.AddCommentAsync(taskId, content);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка при добавлении комментария.");
            }

            return RedirectToAction("ViewTask", new { id = taskId, projectId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, int projectId, ProjectTaskStatus taskStatus)
        {
            if (!await _userAccessService.IsCurrentUserExecutorWithTaskAccessAsync(id))
            {
                return NotFound("Нет доступа к задаче.");
            }

            try
            {
                await _taskService.ChangeTaskStatusAsync(id, taskStatus);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка при обновлении задачи.");
            }

            return RedirectToAction("Index", new { projectId });
        }

        private async Task<IActionResult> GetCreateReadTaskVM(int projectId, int? taskId)
        {
            try
            {
                var model = await _taskService.GetCreateReadTaskVMAsync(projectId, taskId);
                return View(model);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка получения данных для создания или редактирования задачи.");
            }
        }
    }
}