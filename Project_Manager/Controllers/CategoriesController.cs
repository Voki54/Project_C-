using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Project_Manager.Data;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.ViewModels;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CategoriesController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Categories/Create
        public IActionResult Create(int projectId)
        {
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin)
            {
                return NotFound();
            }
            ViewBag.ProjectId = projectId;
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == category.ProjectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index", "ProjectTasks", new { projectId = category.ProjectId});
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == category.ProjectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin)
            {
                return NotFound();
            }
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == category.ProjectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin)
            {
                return NotFound();
            }
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index", "ProjectTasks", new { projectId = category.ProjectId }); // Перенаправление на страницу задач
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.Find(id);
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == category.ProjectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin)
            {
                return NotFound();
            }
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "ProjectTasks", new { projectId = category.ProjectId }); // Перенаправление на страницу задач
        }
    }
}
