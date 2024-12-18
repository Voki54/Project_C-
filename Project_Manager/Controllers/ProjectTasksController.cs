using Microsoft.AspNetCore.Mvc;
using Project_Manager.Models;
using Project_Manager.ViewModels;
using Project_Manager.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Project_Manager.Services.Interfaces;
using Project_Manager.Services;
using System.Threading.Tasks;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class ProjectTasksController : Controller
    {
        private readonly IProjectTasksService _taskService;
        private readonly IUserAccessService _userAccessService;
        private readonly ILogger<ProjectTasksController> _logger;

        public ProjectTasksController(IProjectTasksService taskService, IUserAccessService userAccessService, ILogger<ProjectTasksController> logger)
        {
            _taskService = taskService;
            _userAccessService = userAccessService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int projectId, int? categoryId, string? sortColumn, string? filterStatus, string? filterExecutor, DateTime? filterDate)
        {
            _logger.LogInformation($"Вызван метод Index с параметрами: projectId = {projectId}, categoryId = {categoryId}, " +
            $"sortColumn = {sortColumn}, filterStatus = {filterStatus}, filterExecutor = {filterExecutor}, filterDate = {filterDate}");

            if (!await _userAccessService.IsCurrentUserExecutorOrManagerOrAdminWithProjectAccessAsync(projectId))
            {
                _logger.LogError($"Пользователь не имеет доступа к проекту с ID: {projectId}");
                return NotFound("Нет доступа к проекту.");
            }

            TaskCategoryVM model;

            try
            {
                model = await _taskService.GetTaskCategoryVMAsync(projectId, categoryId, sortColumn, filterStatus, filterExecutor, filterDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении задач проекта.");
                return StatusCode(500, "Ошибка при получении задач проекта.");
            }

            _logger.LogInformation($"Задачи для проекта с ID {projectId} успешно получены.");
            return View(model);
        }


        public async Task<IActionResult> Create(int projectId)
        {
            _logger.LogInformation($"Вызван метод Create (GET) с параметром: projectId = {projectId}");
            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithProjectAccessAsync(projectId))
            {
                _logger.LogError($"Пользователь не имеет доступа к проекту с ID: {projectId}");
                return NotFound("Нет доступа к проекту.");
            }

            return await GetCreateReadTaskVM(projectId, null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectTask task, int projectId)
        {
            _logger.LogInformation("Вызван метод Create (POST) с projectId: {Id}, " +
                "задачей: {@ProjectTask}", projectId, task);

            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithProjectAccessAsync(projectId))
            {
                _logger.LogError($"Пользователь не имеет доступа к проекту с ID: {projectId}");
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
                    _logger.LogError(ex, "Ошибка при добавлении задачи: {@ProjectTask}", task);
                    return StatusCode(500, "Ошибка при добавлении задачи.");
                }
                _logger.LogInformation($"Задача успешно создана для проекта с ID: {projectId}");
                return RedirectToAction("Index", new { projectId }); 
            }

            _logger.LogWarning("Модель задачи невалидна {@ProjectTask}", task);
            return await GetCreateReadTaskVM(projectId, null);
        }


        public async Task<IActionResult> Edit(int id, int projectId)
        {
            _logger.LogInformation($"Вызван метод Edit (GET) с параметрами: " +
                $"projectId = {projectId}, Id = {id}");

            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithTaskAccessAsync(id))
            {
                _logger.LogError($"Пользователь не имеет доступа к задаче с ID: {id}");
                return NotFound("Нет доступа к задаче.");
            }

            return await GetCreateReadTaskVM(projectId, id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectTask task, int projectId)
        {
            _logger.LogInformation("Вызван метод Edit (POST) с id: {Id} ," +
                "projectId: {projectId}, задачей: {@ProjectTask}", id, projectId, task);

            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithTaskAccessAsync(id) || (id != task.Id))
            {
                _logger.LogError($"Пользователь не имеет доступа к задаче с ID: {id}");
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
                    _logger.LogError(ex, "Ошибка при обновлении задачи: {@ProjectTask}", task);
                    return StatusCode(500, "Ошибка при обновлении задачи.");
                }
                _logger.LogInformation($"Задача успешно обновлена для проекта с ID: {projectId}");
                return RedirectToAction("Index", new { projectId });
            }

            _logger.LogWarning("Модель задачи невалидна {@ProjectTask}", task);
            return await GetCreateReadTaskVM(projectId, id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int projectId)
        {
            _logger.LogInformation("Вызван метод DeleteConfirmed (POST)" +
                " с id: {Id}, projectId: {projectId}", id, projectId);

            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithTaskAccessAsync(id))
            {
                _logger.LogError($"Пользователь не имеет доступа к задаче с ID: {id}");
                return NotFound("Нет доступа к задаче.");
            }

            try
            {
                await _taskService.DeleteTaskAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Задача с ID {ProjectTaskId} не найдена при удалении", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении задачи с Id: {ProjectTaskId}", id);
                return StatusCode(500, "Ошибка при удалении задачи.");
            }

            _logger.LogInformation("Задача с ID {ProjectTaskId} успешно удалена", id);
            return RedirectToAction("Index", new { projectId });
        }


        public async Task<IActionResult> ViewTask(int id, int projectId)
        {
            _logger.LogInformation("Вызван метод ViewTask (GET)" +
                " с id: {Id} ,projectId: {projectId}", id, projectId);

            if (!await _userAccessService.IsCurrentUserExecutorOrManagerOrAdminWithTaskAccessAsync(id))
            {
                _logger.LogError($"Пользователь не имеет доступа к задаче с ID: {id}");
                return NotFound("Нет доступа к задаче.");
            }

            ViewTaskVM model = null;

            try
            {
                model = await _taskService.GetViewTaskVMAsync(id, projectId);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Задача с ID {ProjectTaskId} не найдена", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении задачи с Id: {ProjectTaskId}", id);
                return StatusCode(500, "Ошибка при получении задачи.");
            }

            _logger.LogInformation("Задача с ID {ProjectTaskId} успешно получена", id);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddComment(int taskId, string content, int projectId)
        {
            _logger.LogInformation("Вызван метод AddComment (POST) с id: {Id} ," +
                "projectId: {projectId}, content: {content}", taskId, projectId, content);

            if (!await _userAccessService.IsCurrentUserExecutorWithTaskAccessAsync(taskId))
            {
                _logger.LogError($"Пользователь не имеет доступа к задаче с ID: {taskId}");
                return NotFound("Нет доступа к задаче.");
            }

            try
            {
                await _taskService.AddCommentAsync(taskId, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении комментария к задаче с Id: {ProjectTaskId}", taskId);
                return StatusCode(500, "Ошибка при добавлении комментария.");
            }

            _logger.LogInformation("Комментарий к задаче с ID {ProjectTaskId} успешно добавлен", taskId);
            return RedirectToAction("ViewTask", new { id = taskId, projectId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, int projectId, ProjectTaskStatus taskStatus)
        {
            _logger.LogInformation("Вызван метод ChangeStatus (POST) с id: {Id} ," +
                "projectId: {projectId}, taskStatus: {taskStatus}", id, projectId, taskStatus);

            if (!await _userAccessService.IsCurrentUserExecutorWithTaskAccessAsync(id))
            {
                _logger.LogError($"Пользователь не имеет доступа к задаче с ID: {id}");
                return NotFound("Нет доступа к задаче.");
            }

            try
            {
                await _taskService.ChangeTaskStatusAsync(id, taskStatus);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Задача с ID {ProjectTaskId} не найдена", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении статуса задачи с Id: {ProjectTaskId}", id);
                return StatusCode(500, "Ошибка при обновлении статуса задачи.");
            }

            _logger.LogInformation("Статус задачи с ID {ProjectTaskId} успешно обновлен", id);
            return RedirectToAction("Index", new { projectId });
        }

        private async Task<IActionResult> GetCreateReadTaskVM(int projectId, int? taskId)
        {
            _logger.LogInformation($"Вызван метод GetCreateReadTaskVM с параметрами: " +
                $"projectId = {projectId}, taskId = {taskId}");

            try
            {
                var model = await _taskService.GetCreateReadTaskVMAsync(projectId, taskId);
                _logger.LogInformation($"Модель успешно получена для проекта с ID {projectId} и задачи с ID {taskId}.");
                return View(model);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Не удалось найти данные для редактирования задачи с ID {taskId} в проекте с ID {projectId}.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения данных для создания или редактирования задачи.");
                return StatusCode(500, "Ошибка получения данных для создания или редактирования задачи.");
            }
        }
    }
}