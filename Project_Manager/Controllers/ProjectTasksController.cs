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

        public IActionResult Index(int projectId, int? categoryId, string? sortColumn, string? filterStatus, string? filterExecutor, DateTime? filterDate)
        {
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Executor && projectUser.Role != UserRoles.Admin)
            {
                return NotFound();
            }
            // Получаем список всех категорий
            var categories = _context.Categories.Where(t => t.ProjectId == projectId).ToList();
            if (categories == null || !categories.Any())
            {
                Console.WriteLine("Список категорий пуст.");
            }
            else
            {
                Console.WriteLine("Список категорий:");
                foreach (var category in categories)
                {
                    Console.WriteLine($"- {category.Name} (ID: {category.Id})");
                }
            }

            List<ProjectTaskDTO> tasks;

            if(projectUser.Role != UserRoles.Executor)
            {
                if (sortColumn != null)
                {
                    var isAscending = !SortState.isColumnInProjectTaskViewSorted.GetValueOrDefault(sortColumn, false);
                    var orderBy = isAscending ? sortColumn : sortColumn + " desc";

                    if (sortColumn == "Status")
                        tasks = _context.Tasks
                            .Include(t => t.AppUser)
                            .Include(t => t.Category)
                            .Where(t => t.Category.ProjectId == projectId)
                            .OrderBy(orderBy)
                            .Select(t => new ProjectTaskDTO
                            {
                                Id = t.Id,
                                Title = t.Title,
                                Status = t.Status.HasValue ? t.Status.ToString() : "Не указан",
                                Category = t.Category,
                                ExecutorName = t.AppUser != null ? t.AppUser.UserName : "Не назначен",
                                DueDateTime = t.DueDateTime,
                                Description = t.Description
                            })
                            .ToList();
                    else
                        tasks = _context.Tasks
                            .Include(t => t.AppUser)
                            .Include(t => t.Category)
                            .Where(t => t.Category.ProjectId == projectId)
                            .Select(t => new ProjectTaskDTO
                            {
                                Id = t.Id,
                                Title = t.Title,
                                Status = t.Status.HasValue ? t.Status.ToString() : "Не указан",
                                Category = t.Category,
                                ExecutorName = t.AppUser != null ? t.AppUser.UserName : "Не назначен",
                                DueDateTime = t.DueDateTime,
                                Description = t.Description
                            })
                            .OrderBy(orderBy)
                            .ToList();


                    SortState.isColumnInProjectTaskViewSorted[sortColumn] = isAscending;
                }
                else
                {
                    tasks = _context.Tasks
                        .Include(t => t.AppUser)
                        .Include(t => t.Category)
                        .Where(t => t.Category.ProjectId == projectId)
                        .Select(t => new ProjectTaskDTO
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Status = t.Status.HasValue ? t.Status.ToString() : "Не указан",
                            Category = t.Category,
                            ExecutorName = t.AppUser != null ? t.AppUser.UserName : "Не назначен",
                            DueDateTime = t.DueDateTime,
                            Description = t.Description
                        })
                        .ToList();
                }
            }
            else
            {
                if (sortColumn != null)
                {
                    var isAscending = !SortState.isColumnInProjectTaskViewSorted.GetValueOrDefault(sortColumn, false);
                    var orderBy = isAscending ? sortColumn : sortColumn + " desc";

                    if (sortColumn == "Status")
                        tasks = _context.Tasks
                            .Include(t => t.AppUser)
                            .Include(t => t.Category)
                            .Where(t => t.Category.ProjectId == projectId)
                            .Where(t => t.ExecutorId == projectUser.UserId)
                            .OrderBy(orderBy)
                            .Select(t => new ProjectTaskDTO
                            {
                                Id = t.Id,
                                Title = t.Title,
                                Status = t.Status.HasValue ? t.Status.ToString() : "Не указан",
                                Category = t.Category,
                                ExecutorName = t.AppUser != null ? t.AppUser.UserName : "Не назначен",
                                DueDateTime = t.DueDateTime,
                                Description = t.Description
                            })
                            .ToList();
                    else
                        tasks = _context.Tasks
                            .Include(t => t.AppUser)
                            .Include(t => t.Category)
                            .Where(t => t.Category.ProjectId == projectId)
                            .Where(t => t.ExecutorId == projectUser.UserId)
                            .Select(t => new ProjectTaskDTO
                            {
                                Id = t.Id,
                                Title = t.Title,
                                Status = t.Status.HasValue ? t.Status.ToString() : "Не указан",
                                Category = t.Category,
                                ExecutorName = t.AppUser != null ? t.AppUser.UserName : "Не назначен",
                                DueDateTime = t.DueDateTime,
                                Description = t.Description
                            })
                            .OrderBy(orderBy)
                            .ToList();


                    SortState.isColumnInProjectTaskViewSorted[sortColumn] = isAscending;
                }
                else
                {
                    tasks = _context.Tasks
                        .Include(t => t.AppUser)
                        .Include(t => t.Category)
                        .Where(t => t.Category.ProjectId == projectId)
                        .Where(t => t.ExecutorId == projectUser.UserId)
                        .Select(t => new ProjectTaskDTO
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Status = t.Status.HasValue ? t.Status.ToString() : "Не указан",
                            Category = t.Category,
                            ExecutorName = t.AppUser != null ? t.AppUser.UserName : "Не назначен",
                            DueDateTime = t.DueDateTime,
                            Description = t.Description
                        })
                        .ToList();
                }
            }
            

            Category selectedCategory = null;

            if (categoryId.HasValue)
            {
                selectedCategory = _context.Categories.Find(categoryId.Value);

                if (selectedCategory != null)
                {
                    tasks = tasks.Where(t => t.Category.Id == selectedCategory.Id).ToList();
                }
                else
                {
                    Console.WriteLine("Выбранная категория не найдена.");
                }
            }
            if (filterStatus != null)
            {
                tasks = tasks.Where(t => t.Status == filterStatus).ToList();
            }
            if (filterExecutor != null)
            {
                tasks = tasks.Where(t => t.ExecutorName == filterExecutor).ToList();
            }
            if (filterDate != null)
            {
                tasks = tasks.Where(t => t.DueDateTime <= filterDate).ToList();
            }

            var model = new TaskCategoryVM
            {
                Categories = categories ?? new List<Category>(),
                SelectedCategory = selectedCategory ?? new Category(),
                Tasks = tasks ?? new List<ProjectTaskDTO>(),
                SortedColumn = sortColumn,
                IsAsc = sortColumn != null ? !SortState.isColumnInProjectTaskViewSorted.GetValueOrDefault(sortColumn, false) : null,
                ProjectId = projectId,
                Role = projectUser.Role
            };

            return View(model);
        }


        public IActionResult Create(int projectId)
        {
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin)
            {
                return NotFound();
            }

            var projectUsers = _context.ProjectsUsers
                                    .Where(pu => pu.ProjectId == projectId)
                                    .Select(pu => pu.AppUser)
                                    .ToList();
            var categories = _context.Categories.Where(t => t.ProjectId == projectId).ToList();


            ViewBag.ProjectId = projectId;
            ViewBag.Categories = categories; 
            ViewBag.Users = projectUsers;          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectTask task, int projectId)
        {
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin)
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
                                    .Select(pu => pu.AppUser)
                                    .ToListAsync();
            var categories = await _context.Categories.Where(t => t.ProjectId == projectId).ToListAsync();

            ViewBag.ProjectId = projectId;
            ViewBag.Categories = categories;
            ViewBag.Users = projectUsers;
            return View(task);
        }


        // GET: ProjectTasks/Edit/5
        public async Task<IActionResult> Edit(int id, int projectId)
        {
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin)
            {
                return NotFound();
            }
            var projectTask = await _context.Tasks.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }
            var projectUsers = await _context.ProjectsUsers
                                    .Where(pu => pu.ProjectId == projectId)
                                    .Select(pu => pu.AppUser)
                                    .ToListAsync();
            var categories = await _context.Categories.Where(t => t.ProjectId == projectId).ToListAsync();

            ViewBag.ProjectId = projectId;
            ViewBag.Categories = categories;
            ViewBag.Users = projectUsers;
            return View(projectTask);
        }

        // POST: ProjectTasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectTask projectTask, int projectId)
        {
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin)
            {
                return NotFound();
            }
            if (id != projectTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectTaskExists(projectTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { projectId });
            }

            var projectUsers = await _context.ProjectsUsers
                                    .Where(pu => pu.ProjectId == projectId)
                                    .Select(pu => pu.AppUser)
                                    .ToListAsync();
            var categories = await _context.Categories.Where(t => t.ProjectId == projectId).ToListAsync();

            ViewBag.ProjectId = projectId;
            ViewBag.Categories = categories;
            ViewBag.Users = projectUsers;
            return View(projectTask);
        }

        // POST: ProjectTasks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int projectId)
        {
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Admin)
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

        // Метод для просмотра задачи
        public IActionResult ViewTask(int id, int projectId)
        {
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Manager && projectUser.Role != UserRoles.Executor && projectUser.Role != UserRoles.Admin)
            {
                return NotFound();
            }
            var task = _context.Tasks
                .Include(t => t.Comments)
                .Include(t => t.AppUser)
                .Include(t => t.Category)
                .FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            var taskDAO = new ProjectTaskDTO
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
            ViewBag.ProjectId = projectId;
            ViewBag.Role = projectUser.Role;
            return View(taskDAO);
        }

        // Метод для добавления комментария
        [HttpPost]
        public IActionResult AddComment(int taskId, string content, int projectId)
        {
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (projectUser == null || projectUser.Role != UserRoles.Executor)
            {
                return NotFound();
            }
            var task = _context.Tasks.Find(taskId);
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
            _context.SaveChanges();

            return RedirectToAction("ViewTask", new { id = taskId, projectId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, int projectId, ProjectTaskStatus? taskStatus)
        {
            var projectUser = _context.ProjectsUsers.FirstOrDefault(pu => pu.ProjectId == projectId && pu.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
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

        private bool ProjectTaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}