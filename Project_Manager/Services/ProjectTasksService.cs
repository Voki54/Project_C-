using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Helpers;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.ViewModels;
using System.Security.Claims;

namespace Project_Manager.Services
{
    public class ProjectTasksService
    {
        private readonly IProjectTaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectTasksService(IProjectTaskRepository taskRepository, IProjectRepository projectRepository, 
            IProjectUserRepository projectUserRepository, ICommentRepository commentRepository, 
            ICategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _projectUserRepository = projectUserRepository;
            _commentRepository = commentRepository;
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateReadTaskVM> GetCreateReadTaskVMAsync(int projectId, int? taskId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                throw new KeyNotFoundException($"Проект с ID {projectId} не найден.");
            }
            var model = await ToCreateReadTaskVM(projectId, taskId);
            return model;
        }

        public async Task<string> CurrentUserIdAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }

        public async Task<UserRoles?> CurrentUserRoleInProjectOrNullAsync(int projectId)
        {
            var userId = await CurrentUserIdAsync();
            return await _projectUserRepository.GetUserRoleInProjectAsync(userId, projectId);
        }

        public async Task<bool> IsCurrentUserManagerOrAdminWithProjectAccessAsync(int projectId)
        {
            var userRole = await CurrentUserRoleInProjectOrNullAsync(projectId);
            return userRole == UserRoles.Manager || userRole == UserRoles.Admin;
        }

        public async Task<bool> IsCurrentUserManagerOrAdminWithTaskAccessAsync(int taskId)
        {
            var task = await FindTaskByIdOrNullIncludeUsersAndCategoriesAsync(taskId);
            var userRole = await CurrentUserRoleInProjectOrNullAsync(task.Category.Project.Id);
            return userRole == UserRoles.Manager || userRole == UserRoles.Admin;
        }

        public async Task<ProjectTask> CreateTaskAsync(ProjectTask task)
        {
            return await _taskRepository.CreateAsync(task);
        }

        public async Task<ProjectTask> UpdateTaskAsync(ProjectTask task)
        {
            return await _taskRepository.UpdateAsync(task);
        }

        public async Task<ProjectTask> FindTaskByIdOrNullAsync(int taskId)
        {
            return await _taskRepository.FindByIdOrNullAsync(taskId);
        }

        public async Task<ProjectTask> FindTaskByIdOrNullIncludeUsersAndCategoriesAsync(int taskId)
        {
            return await _taskRepository.FindByIdOrNullIncludeUsersAndCategoriesAsync(taskId);
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            await _taskRepository.DeleteByIdAsync(taskId);
        }

        public async Task<bool> IsCurrentUserExecutorWithTaskAccessAsync(int taskId)
        {
            var projectTask = await FindTaskByIdOrNullAsync(taskId);
            if (projectTask == null) return false;
            var taskExecutor = projectTask.ExecutorId;
            var userId = await CurrentUserIdAsync();
            return userId == taskExecutor;
        }

        public async Task<bool> IsCurrentUserExecutorOrManagerOrAdminWithTaskAccessAsync(int taskId)
        {
            var isManagerOrAdmin = await IsCurrentUserManagerOrAdminWithTaskAccessAsync(taskId);
            var isExecutor = await IsCurrentUserExecutorWithTaskAccessAsync(taskId);
            return isExecutor || isManagerOrAdmin;
        }

        public async Task<bool> IsCurrentUserExecutorOrManagerOrAdminWithProjectAccessAsync(int projectId)
        {
            var userRole = await CurrentUserRoleInProjectOrNullAsync(projectId);
            return userRole == UserRoles.Manager || userRole == UserRoles.Admin || userRole == UserRoles.Executor;
        }

        public async Task ChangeTaskStatusAsync(int taskId, ProjectTaskStatus taskStatus)
        {
            await _taskRepository.ChangeStatusByIdAsync(taskId, taskStatus);
        }

        public async Task<Comment> AddCommentAsync(int taskId, string content)
        {
            var comment = new Comment
            {
                Content = content,
                ProjectTaskId = taskId,
                CreatedAt = DateTime.Now
            };
            await _commentRepository.CreateAsync(comment);
            return comment;
        }

        public async Task<CreateReadTaskVM> ToCreateReadTaskVM(int projectId, int? taskId)
        {
            ProjectTask task = null;
            if (taskId != null)
            {
                task = await _taskRepository.FindByIdOrNullAsync(taskId);
                if(task == null) throw new KeyNotFoundException($"Задача с ID {taskId} не найдена.");
            }
            var projectUsers = await _projectUserRepository.GetUsersDtoByProjectAsync(projectId);
            var categories = await _categoryRepository.GetCategoriesByProjectIdAsync(projectId);
            var model = new CreateReadTaskVM
            {
                Task = task,
                ProjectId = projectId,
                Categories = categories,
                Users = projectUsers
            };
            return model;
        }

        public async Task<ViewTaskVM?> GetViewTaskVMAsync(int taskId, int projectId)
        {
            var taskDTO = await _taskRepository.GetTaskDtoByIdAsync(taskId);
            if (taskDTO == null)
            {
                throw new KeyNotFoundException($"Задача с ID {taskId} не найдена.");
            }
            var model = new ViewTaskVM
            {
                Task = taskDTO,
                ProjectId = projectId,
                Role = (UserRoles)await CurrentUserRoleInProjectOrNullAsync(projectId),
            };
            return model;
        }

        public async Task<TaskCategoryVM> GetTaskCategoryVMAsync(int projectId, int? categoryId, string? sortColumn, 
            string? filterStatus, string? filterExecutor, DateTime? filterDate)
        {
            var categories = await _categoryRepository.GetCategoriesByProjectIdAsync(projectId);
            var currUserId = await CurrentUserIdAsync();
            var role = await CurrentUserRoleInProjectOrNullAsync(projectId);
            string orderBy = null;
            if (sortColumn != null)
            {
                var isAscending = !SortState.isColumnInProjectTaskViewSorted.GetValueOrDefault(sortColumn, false);
                orderBy = isAscending ? sortColumn : sortColumn + " desc";

                SortState.isColumnInProjectTaskViewSorted[sortColumn] = isAscending;
            }
            var tasks = await _taskRepository.GetTasksDtoWithParamsAsync(projectId, categoryId, orderBy, currUserId, 
                role, filterStatus, filterExecutor, filterDate);

            var model = new TaskCategoryVM
            {
                Categories = categories,
                SelectedCategory = categoryId,
                Tasks = tasks,
                SortedColumn = sortColumn,
                IsAsc = sortColumn != null ? SortState.isColumnInProjectTaskViewSorted.GetValueOrDefault(sortColumn, false) : null,
                ProjectId = projectId,
                Role = (UserRoles)role
            };
            return model;
        }
    }
}
