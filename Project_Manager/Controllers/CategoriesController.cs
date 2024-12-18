using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Models;
using Project_Manager.Services;
using Project_Manager.Services.Interfaces;
using System.Threading.Tasks;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly IUserAccessService _userAccessService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IUserAccessService userAccessService, 
            ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _userAccessService = userAccessService;
            _categoryService = categoryService;
            _logger = logger;
        }


        public async Task<IActionResult> Create(int projectId)
        {
            _logger.LogInformation("Вызван метод Create (GET) с projectId: {ProjectId}", projectId);
            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithProjectAccessAsync(projectId))
            {
                _logger.LogError("Пользователь не имеет доступа к проекту с ID {ProjectId}", projectId);
                return NotFound("Нет доступа к проекту.");
            }

            ViewBag.ProjectId = projectId;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            _logger.LogInformation("Вызван метод Create (POST) с категорией: {@Category}", category);
            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithProjectAccessAsync(category.ProjectId))
            {
                _logger.LogError("Пользователь не имеет доступа к проекту с ID {ProjectId}", category.ProjectId);
                return NotFound("Нет доступа к проекту.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.CreateCategoryAsync(category);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при добавлении категории: {@Category}", category);
                    return StatusCode(500, "Ошибка при добавлении категории.");
                }
                _logger.LogInformation("Категория c ID {CategoryId} успешно создана", category.Id);
                return RedirectToAction("Index", "ProjectTasks", new { projectId = category.ProjectId});
            }

            _logger.LogWarning("Модель категории невалидна {@Category}", category);
            ViewBag.ProjectId = category.ProjectId;
            return View(category);
        }


        public async Task<IActionResult> Edit(int id, int projectId)
        {
            _logger.LogInformation("Вызван метод Edit (GET) с id: {Id}, " +
                "projectId: {ProjectId}", id, projectId);

            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithCategoryAccessAsync(id))
            {
                _logger.LogError("Пользователь не имеет доступа к категории с ID {CategoryId}", id);
                return NotFound("Нет доступа к категории.");
            }

            ViewBag.ProjectId = projectId;
            return await GetEditModel(id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category, int projectId)
        {
            _logger.LogInformation("Вызван метод Edit (POST) с id: {Id}, категорией: {@Category}, " +
                "projectId: {projectId}", id, category, projectId);

            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithCategoryAccessAsync(id) || (id != category.Id))
            {
                _logger.LogError("Пользователь не имеет доступа к категории с ID {CategoryId}", id);
                return NotFound("Нет доступа к категории.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.UpdateCategoryAsync(category);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при обновлении категории: {@Category}", category);
                    return StatusCode(500, "Ошибка при обновлении категории.");
                }
                _logger.LogInformation("Категория c ID {CategoryId} успешно обновлена", category.Id);
                return RedirectToAction("Index", "ProjectTasks", new { projectId });
            }

            _logger.LogWarning("Модель категории невалидна {@Category}", category);
            ViewBag.ProjectId = projectId;
            return await GetEditModel(category.Id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int projectId)
        {
            _logger.LogInformation("Вызван метод DeleteConfirmed с id: {Id}, " +
                "projectId: {ProjectId}", id, projectId);

            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithCategoryAccessAsync(id))
            {
                _logger.LogError("Пользователь не имеет доступа к категории с ID {CategoryId}", id);
                return NotFound("Нет доступа к категории.");
            }

            try
            {
                await _categoryService.DeleteCategoryAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Категория с ID {CategoryId} не найдена при удалении", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении категории с ID {CategoryId}", id);
                return StatusCode(500, "Ошибка при удалении категории.");
            }

            _logger.LogInformation("Категория с ID {CategoryId} успешно удалена", id);
            return RedirectToAction("Index", "ProjectTasks", new { projectId }); 
        }

        private async Task<IActionResult> GetEditModel(int categoryId)
        {
            _logger.LogInformation($"Запрос на получение модели редактирования категории с ID: {categoryId}");
            try
            {
                var category = await _categoryService.FindCategoryByIdAsNoTrackingAsync(categoryId);
                _logger.LogInformation($"Категория с ID {categoryId} успешно получена.");
                return View(category);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, $"Ошибка: категория с ID {categoryId} не найдена.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения данных для редактирования категории.");
                return StatusCode(500, "Ошибка получения данных для редактирования категории.");
            }
        }
    }
}
