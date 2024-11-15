using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Models;
using Project_Manager.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Manager.Controllers
{
    public class ProjectTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? categoryId)
        {

            // Получаем список всех категорий
            var categories = _context.Categories.ToList();
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

            var tasksQuery = _context.Tasks.AsQueryable();

            // Включаем зависимость AppUser (исполнителя) и Category (категории)
            tasksQuery = tasksQuery
                .Include(t => t.AppUser)    // Включаем исполнителя
                .Include(t => t.Category);  // Включаем категорию

            var tasks = tasksQuery.ToList();
            if (tasks == null || !tasks.Any())
            {
                Console.WriteLine("Список задач пуст.");
            }
            else
            {
                Console.WriteLine("Список задач:");
                foreach (var task in tasks)
                {
                    Console.WriteLine($"- {task.Title} (ID: {task.Id}, Статус: {task.Status})");
                }
            }

            Category selectedCategory = null;

            if (categoryId.HasValue)
            {
                selectedCategory = _context.Categories.Find(categoryId.Value);

                if (selectedCategory != null)
                {
                    tasks = _context.Tasks.Where(t => t.CategoryId == selectedCategory.Id).ToList();
                }
                else
                {
                    Console.WriteLine("Выбранная категория не найдена.");
                }
            }

            var model = new TaskCategoryVM
            {
                Categories = categories ?? new List<Category>(), 
                SelectedCategory = selectedCategory ?? new Category(),
                Tasks = tasks ?? new List<ProjectTask>()
            };

            return View(model);
        }


        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList(); 
            ViewBag.Users = _context.Users.ToList();          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                await _context.Tasks.AddAsync(task); 
                await _context.SaveChangesAsync();
                return RedirectToAction("Index"); 
            }

            ViewBag.Categories = await _context.Categories.ToListAsync(); 
            ViewBag.Users = await _context.Users.ToListAsync();
            return View(task);
        }


        // GET: ProjectTasks/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var projectTask = await _context.Tasks.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Users = await _context.Users.ToListAsync();
            return View(projectTask);
        }

        // POST: ProjectTasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectTask projectTask)
        {
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
                return RedirectToAction("Index");
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Users = await _context.Users.ToListAsync();
            return View(projectTask);
        }

        // POST: ProjectTasks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectTask = await _context.Tasks.FindAsync(id);
            if (projectTask != null)
            {
                _context.Tasks.Remove(projectTask);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Метод для просмотра задачи
        public IActionResult ViewTask(int id)
        {
            var task = _context.Tasks
                .Include(t => t.Comments)
                .FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // Метод для добавления комментария
        [HttpPost]
        public IActionResult AddComment(int taskId, string content)
        {
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

            return RedirectToAction("ViewTask", new { id = taskId });
        }


        private bool ProjectTaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }

    }
}