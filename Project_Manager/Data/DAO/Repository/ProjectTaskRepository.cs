using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.DTO.ProjectTasks;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using System.Linq.Dynamic.Core;

namespace Project_Manager.Data.DAO.Repository
{
    public class ProjectTaskRepository:IProjectTaskRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectTaskRepository> _logger;

        public ProjectTaskRepository(ApplicationDbContext context, ILogger<ProjectTaskRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ProjectTask> CreateAsync(ProjectTask task)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: {@Task}", nameof(CreateAsync), task);
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Задача с ID {TaskId} успешно создана.", task.Id);
            return task;
        }

        public async Task<ProjectTask> UpdateAsync(ProjectTask task)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: {@Task}", nameof(UpdateAsync), task);
            _context.Update(task);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Задача с ID {TaskId} успешно обновлена.", task.Id);
            return task;
        }

        public async Task<ProjectTask?> FindByIdOrNullAsync(int? taskId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}", nameof(FindByIdOrNullAsync), taskId);
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<ProjectTask?> FindByIdOrNullAsNoTrackingAsync(int? taskId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}", nameof(FindByIdOrNullAsNoTrackingAsync), taskId);
            return await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<ProjectTask?> FindByIdOrNullIncludeUsersAndCategoriesAsync(int taskId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}", nameof(FindByIdOrNullIncludeUsersAndCategoriesAsync), taskId);
            return await _context.Tasks
                        .Include(t => t.AppUser)
                        .Include(t => t.Category)
                            .ThenInclude(c => c.Project)
                        .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<ProjectTask?> FindByIdOrNullIncludeUsersAndCategoriesAsNoTrackingAsync(int taskId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}", nameof(FindByIdOrNullIncludeUsersAndCategoriesAsNoTrackingAsync), taskId);
            return await _context.Tasks
                        .Include(t => t.AppUser)
                        .Include(t => t.Category)
                            .ThenInclude(c => c.Project)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<ProjectTaskDTO?> GetTaskDtoByIdAsync(int taskId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}", nameof(GetTaskDtoByIdAsync), taskId);
            return await _context.Tasks
                            .Include(t => t.Comments)
                            .Include(t => t.AppUser)
                            .Include(t => t.Category)
                            .Select(t => new ProjectTaskDTO
                            {
                                Id = t.Id,
                                Title = t.Title,
                                Status = t.Status.ToString(),
                                Category = t.Category,
                                ExecutorName = t.AppUser.UserName,
                                DueDateTime = t.DueDateTime,
                                Description = t.Description,
                                Comments = t.Comments,
                            })
                            .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task DeleteByIdAsync(int taskId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}", nameof(DeleteByIdAsync), taskId);
            ProjectTask projectTask = await FindByIdOrNullAsync(taskId);
            if (projectTask == null)
            {
                _logger.LogError("Задача с ID {TaskId} не найдена для удаления.", taskId);
                throw new KeyNotFoundException($"Задача с ID {taskId} не найдена.");
            }
            _context.Tasks.Remove(projectTask);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Задача с ID {TaskId} успешно удалена.", taskId);
        }

        public async Task ChangeStatusByIdAsync(int taskId, ProjectTaskStatus taskStatus)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}, taskStatus = {TaskStatus}", nameof(ChangeStatusByIdAsync), taskId, taskStatus);
            ProjectTask projectTask = await FindByIdOrNullAsync(taskId);
            if (projectTask == null)
            {
                _logger.LogError("Задача с ID {TaskId} не найдена для изменения статуса.", taskId);
                throw new KeyNotFoundException($"Задача с ID {taskId} не найдена.");
            }
            projectTask.Status = taskStatus;
            _context.Update(projectTask);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Статус задачи с ID {TaskId} успешно изменен на {TaskStatus}.", taskId, taskStatus);
        }

        public async Task<List<ProjectTaskDTO>> GetTasksDtoWithParamsAsync(int projectId, int? categoryId, string? sortColumn, 
            string? userId, UserRoles? userRole, string? filterStatus, string? filterExecutor, DateTime? filterDate)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: projectId = {ProjectId}, categoryId = {CategoryId}, " +
                "userId = {User Id}, userRole = {User Role}, filterStatus = {FilterStatus}, filterExecutor = {FilterExecutor}, " +
                "filterDate = {FilterDate}", nameof(GetTasksDtoWithParamsAsync), projectId, categoryId, userId, userRole, filterStatus, 
                filterExecutor, filterDate);

            var tasksQuery = _context.Tasks
                                .Include(t => t.AppUser)
                                .Include(t => t.Category)
                                .Where(t => t.Category.ProjectId == projectId);

            if (userRole == UserRoles.Executor)
            {
                tasksQuery = tasksQuery.Where(t => t.ExecutorId == userId);
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
                tasksQuery = tasksQuery.OrderBy(sortColumn);
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

            _logger.LogInformation("Найдено {Count} задач для проекта с ID {ProjectId}.", tasks.Count, projectId);
            return tasks;
        }
    }
}
