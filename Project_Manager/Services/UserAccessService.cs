using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using System.Security.Claims;

namespace Project_Manager.Services
{
    public class UserAccessService
    {
        private readonly IProjectTaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessService(IProjectTaskRepository taskRepository, ICategoryRepository categoryRepository,
        IProjectUserRepository projectUserRepository, IHttpContextAccessor httpContextAccessor)
        {
            _taskRepository = taskRepository;
            _projectUserRepository = projectUserRepository;
            _httpContextAccessor = httpContextAccessor;
            _categoryRepository = categoryRepository;
        }

        public async Task<string?> CurrentUserIdAsync()
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
            var task = await _taskRepository.FindByIdOrNullIncludeUsersAndCategoriesAsNoTrackingAsync(taskId);
            var userRole = await CurrentUserRoleInProjectOrNullAsync(task.Category.Project.Id);
            return userRole == UserRoles.Manager || userRole == UserRoles.Admin;
        }

        public async Task<bool> IsCurrentUserManagerOrAdminWithCategoryAccessAsync(int categoryId)
        {
            Category category = null;
            try
            {
                category = await _categoryRepository.FindByIdAsync(categoryId);
            }
            catch (Exception ex)
            {
                return false;
            }
            var userRole = await CurrentUserRoleInProjectOrNullAsync(category.ProjectId);
            return userRole == UserRoles.Manager || userRole == UserRoles.Admin;
        }

        public async Task<bool> IsCurrentUserExecutorWithTaskAccessAsync(int taskId)
        {
            var projectTask = await _taskRepository.FindByIdOrNullAsync(taskId);
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
    }
}
