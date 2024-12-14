using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.DTO.ProjectTasks;
using Project_Manager.Models;
using Project_Manager.ViewModels;
using Project_Manager.Models.Enums;
using System.Linq.Dynamic.Core;
using Project_Manager.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Project_Manager.DTO.Users;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class ProjectTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProjectTasksController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int projectId, int? categoryId, string? sortColumn, string? filterStatus, string? filterExecutor, DateTime? filterDate)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || (projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Executor && projectUser.Role != UserRoles.Admin))
            {
                return NotFound();
            }

            var categories = await _context.Categories.Where(t => t.ProjectId == projectId).ToListAsync();

            var tasksQuery = _context.Tasks
            .Include(t => t.AppUser)
            .Include(t => t.Category)
            .Where(t => t.Category.ProjectId == projectId);

            if (projectUser.Role == UserRoles.Executor)
            {
                tasksQuery = tasksQuery.Where(t => t.ExecutorId == projectUser.UserId);
            }

            if (filterStatus != null)
            {
                tasksQuery = tasksQuery.Where(t => t.Status == (ProjectTaskStatus)int.Parse(filterStatus));
            } 
            else if (filterExecutor != null)
            {
                tasksQuery = tasksQuery.Where(t => t.AppUser.UserName == filterExecutor);
            } 
            else if (filterDate != null)
            {
                tasksQuery = tasksQuery.Where(t => t.DueDateTime <= filterDate);
            }

            if (categoryId != null)
            {
                tasksQuery = tasksQuery.Where(t => t.Category.Id == categoryId);
            }

            if (sortColumn != null)
            {
                var isAscending = !SortState.isColumnInProjectTaskViewSorted.GetValueOrDefault(sortColumn, false);
                var orderBy = isAscending ? sortColumn : sortColumn + " desc";

                tasksQuery = tasksQuery.OrderBy(orderBy);

                SortState.isColumnInProjectTaskViewSorted[sortColumn] = isAscending;
            }

            var tasks = await tasksQuery.Select(t => new ProjectTaskDTO
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status.ToString(),
                Category = t.Category,
                ExecutorName = t.AppUser.UserName,
                DueDateTime = t.DueDateTime,
                Description = t.Description
            }).ToListAsync();

            var model = new TaskCategoryVM
            {
                Categories = categories,
                SelectedCategory = categoryId,
                Tasks = tasks,
                SortedColumn = sortColumn,
                IsAsc = sortColumn != null ? !SortState.isColumnInProjectTaskViewSorted.GetValueOrDefault(sortColumn, false) : null,
                ProjectId = projectId,
                Role = projectUser.Role
            };

            return View(model);
        }


        public async Task<IActionResult> Create(int projectId)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || (projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin))
            {
                return NotFound();
            }

            var projectUsers = await _context.ProjectsUsers
                                    .Where(pu => pu.ProjectId == projectId)
                                    .Select(pu => new UserDTO
                                    {
                                        Id = pu.AppUser.Id,
                                        UserName = pu.AppUser.UserName
                                    })
                                    .ToListAsync();
            var categories = await _context.Categories.Where(t => t.ProjectId == projectId).ToListAsync();

            var model = new CreateReadTaskVM
            {
                ProjectId = projectId,
                Categories = categories,
                Users = projectUsers
            };
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectTask task, int projectId)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || (projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _context.Tasks.AddAsync(task); 
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { projectId }); 
            }

            var projectUsers = await _context.ProjectsUsers
                                    .Where(pu => pu.ProjectId == projectId)
                                    .Select(pu => new UserDTO
                                    {
                                        Id = pu.AppUser.Id,
                                        UserName = pu.AppUser.UserName
                                    })
                                    .ToListAsync();

            var categories = await _context.Categories.Where(t => t.ProjectId == projectId).ToListAsync();

            var model = new CreateReadTaskVM
            {
                Task = task,
                ProjectId = projectId,
                Categories = categories,
                Users = projectUsers
            };

            return View(model);
        }


        public async Task<IActionResult> Edit(int id, int projectId)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || (projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin))
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            var projectUsers = await _context.ProjectsUsers
                                    .Where(pu => pu.ProjectId == projectId)
                                    .Select(pu => new UserDTO
                                    {
                                        Id = pu.AppUser.Id,
                                        UserName = pu.AppUser.UserName
                                    })
                                    .ToListAsync();

            var categories = await _context.Categories.Where(t => t.ProjectId == projectId).ToListAsync();

            var model = new CreateReadTaskVM
            {
                Task = task,
                ProjectId = projectId,
                Categories = categories,
                Users = projectUsers
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectTask task, int projectId)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || (projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin))
            {
                return NotFound();
            }

            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction("Index", new { projectId });
            }

            var projectUsers = await _context.ProjectsUsers
                                    .Where(pu => pu.ProjectId == projectId)
                                    .Select(pu => new UserDTO
                                    {
                                        Id = pu.AppUser.Id,
                                        UserName = pu.AppUser.UserName
                                    })
                                    .ToListAsync();

            var categories = await _context.Categories.Where(t => t.ProjectId == projectId).ToListAsync();

            var model = new CreateReadTaskVM
            {
                Task = task,
                ProjectId = projectId,
                Categories = categories,
                Users = projectUsers
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int projectId)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || (projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin))
            {
                return NotFound();
            }

            var projectTask = await _context.Tasks.FindAsync(id);

            if (projectTask != null)
            {
                _context.Tasks.Remove(projectTask);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", new { projectId });
        }


        public async Task<IActionResult> ViewTask(int id, int projectId)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || (projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Executor && projectUser.Role != UserRoles.Admin))
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Comments)
                .Include(t => t.AppUser)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            var taskDTO = new ProjectTaskDTO
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status.ToString(),
                Category = task.Category,
                ExecutorName = task.AppUser.UserName,
                DueDateTime = task.DueDateTime,
                Description = task.Description,
                Comments = task.Comments,
            };

            var model = new ViewTaskVM
            {
                Task = taskDTO,
                ProjectId = projectId,
                Role = projectUser.Role,
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddComment(int taskId, string content, int projectId)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Executor)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                Content = content,
                ProjectTaskId = taskId,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewTask", new { id = taskId, projectId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, int projectId, ProjectTaskStatus taskStatus)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Executor)
            {
                return NotFound();
            }

            var projectTask = await _context.Tasks.FindAsync(id);

            if (projectTask == null)
            {
                return NotFound(); 
            }

            projectTask.Status = taskStatus;

            try
            {
                _context.Update(projectTask);
                await _context.SaveChangesAsync(); 
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Ошибка при обновлении задачи.");
            }

            return RedirectToAction("Index", new { projectId });
        }
    }
}