using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
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


        public async Task<IActionResult> Create(int projectId)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || (projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin))
            {
                return NotFound();
            }

            ViewBag.ProjectId = projectId;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == category.ProjectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || (projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ProjectTasks", new { projectId = category.ProjectId});
            }

            return View(category);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == category.ProjectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || (projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin))
            {
                return NotFound();
            }

            ViewBag.ProjectId = category.ProjectId;
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == category.ProjectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || (projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin))
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
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ProjectTasks", new { projectId = category.ProjectId });
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == category.ProjectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || (projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin))
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "ProjectTasks", new { projectId = category.ProjectId }); 
        }
    }
}
