using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Models;
using Project_Manager.Services.Interfaces;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly IUserAccessService _userAccessService;
        private readonly ICategoryService _categoryService;

        public CategoriesController(IUserAccessService userAccessService, ICategoryService categoryService)
        {
            _userAccessService = userAccessService;
            _categoryService = categoryService;
        }


        public async Task<IActionResult> Create(int projectId)
        {
            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithProjectAccessAsync(projectId))
            {
                return NotFound("Нет доступа к проекту.");
            }

            ViewBag.ProjectId = projectId;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithProjectAccessAsync(category.ProjectId))
            {
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
                    return StatusCode(500, "Ошибка при добавлении категории.");
                }
                return RedirectToAction("Index", "ProjectTasks", new { projectId = category.ProjectId});
            }

            ViewBag.ProjectId = category.ProjectId;
            return View(category);
        }


        public async Task<IActionResult> Edit(int id, int projectId)
        {
            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithCategoryAccessAsync(id))
            {
                return NotFound("Нет доступа к категории.");
            }

            ViewBag.ProjectId = projectId;
            return await GetEditModel(id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category, int projectId)
        {
            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithCategoryAccessAsync(id))
            {
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
                    return StatusCode(500, "Ошибка при обновлении категории.");
                }
                return RedirectToAction("Index", "ProjectTasks", new { projectId });
            }

            ViewBag.ProjectId = projectId;
            return await GetEditModel(category.Id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int projectId)
        {
            if (!await _userAccessService.IsCurrentUserManagerOrAdminWithCategoryAccessAsync(id))
            {
                return NotFound("Нет доступа к категории.");
            }

            try
            {
                await _categoryService.DeleteCategoryAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка при удалении категории.");
            }

            return RedirectToAction("Index", "ProjectTasks", new { projectId }); 
        }

        private async Task<IActionResult> GetEditModel(int categoryId)
        {
            try
            {
                var category = await _categoryService.FindCategoryByIdAsNoTrackingAsync(categoryId);
                return View(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка получения данных для создания или редактирования категории.");
            }
        }
    }
}
