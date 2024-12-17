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

        public ProjectTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectTask> CreateAsync(ProjectTask task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<ProjectTask> UpdateAsync(ProjectTask task)
        {
            _context.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<ProjectTask?> FindByIdOrNullAsync(int? taskId)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<ProjectTask?> FindByIdOrNullAsNoTrackingAsync(int? taskId)
        {
            return await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<ProjectTask?> FindByIdOrNullIncludeUsersAndCategoriesAsync(int taskId)
        {
            return await _context.Tasks
                        .Include(t => t.AppUser)
                        .Include(t => t.Category)
                            .ThenInclude(c => c.Project)
                        .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<ProjectTask?> FindByIdOrNullIncludeUsersAndCategoriesAsNoTrackingAsync(int taskId)
        {
            return await _context.Tasks
                        .Include(t => t.AppUser)
                        .Include(t => t.Category)
                            .ThenInclude(c => c.Project)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<ProjectTaskDTO?> GetTaskDtoByIdAsync(int taskId)
        {
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
            ProjectTask projectTask = await FindByIdOrNullAsync(taskId);
            if (projectTask == null)
            {
                throw new KeyNotFoundException($"Задача с ID {taskId} не найдена.");
            }
            _context.Tasks.Remove(projectTask);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeStatusByIdAsync(int taskId, ProjectTaskStatus taskStatus)
        {
            ProjectTask projectTask = await FindByIdOrNullAsync(taskId);
            if (projectTask == null)
            {
                throw new KeyNotFoundException($"Задача с ID {taskId} не найдена.");
            }
            projectTask.Status = taskStatus;
            _context.Update(projectTask);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProjectTaskDTO>> GetTasksDtoWithParamsAsync(int projectId, int? categoryId, string? sortColumn, 
            string? userId, UserRoles? userRole, string? filterStatus, string? filterExecutor, DateTime? filterDate)
        {
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

            return tasks;
        }
    }
}
