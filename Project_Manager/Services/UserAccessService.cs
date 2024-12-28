using NuGet.Protocol.Core.Types;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.Services.Interfaces;
using System.Security.Claims;

namespace Project_Manager.Services
{
    public class UserAccessService: IUserAccessService
    {
        private readonly IProjectTaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserAccessService> _logger;

        public UserAccessService(IProjectTaskRepository taskRepository, ICategoryRepository categoryRepository,
        IProjectUserRepository projectUserRepository, IHttpContextAccessor httpContextAccessor, ILogger<UserAccessService> logger)
        {
            _taskRepository = taskRepository;
            _projectUserRepository = projectUserRepository;
            _httpContextAccessor = httpContextAccessor;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<string?> CurrentUserIdAsync()
        {
            _logger.LogInformation("Вызван метод {MethodName}", nameof(CurrentUserIdAsync));
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("Получен ID текущего пользователя: {User Id}", userId);
            return userId;
        }

        public async Task<UserRoles?> CurrentUserRoleInProjectOrNullAsync(int projectId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: projectId = {ProjectId}", nameof(CurrentUserRoleInProjectOrNullAsync), projectId);
            var userId = await CurrentUserIdAsync();
            var userRole = await _projectUserRepository.GetUserRoleInProjectAsync(userId, projectId);
            _logger.LogInformation("Роль пользователя с ID {User Id} в проекте с ID {ProjectId}: {User Role}", userId, projectId, userRole);
            return userRole;
        }

        public async Task<UserRoles?> CurrentUserRoleInProjectOrTaskExecutorOrNullAsync(int taskId, int projectId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: projectId = {ProjectId}", nameof(CurrentUserRoleInProjectOrNullAsync), projectId);
            var userId = await CurrentUserIdAsync();
            var task = await _taskRepository.FindByIdOrNullAsNoTrackingAsync(taskId);
            if (task != null && task.ExecutorId == userId)
            {
                return UserRoles.Executor;
            }
            var userRole = await _projectUserRepository.GetUserRoleInProjectAsync(userId, projectId);
            _logger.LogInformation("Роль пользователя с ID {User Id} в проекте с ID {ProjectId}: {User Role}", userId, projectId, userRole);
            return userRole;
        }

        public async Task<bool> IsCurrentUserManagerOrAdminWithProjectAccessAsync(int projectId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: projectId = {ProjectId}", nameof(IsCurrentUserManagerOrAdminWithProjectAccessAsync), projectId);
            var userRole = await CurrentUserRoleInProjectOrNullAsync(projectId);
            var hasAccess = userRole == UserRoles.Manager || userRole == UserRoles.Admin;
            _logger.LogInformation("Текущий пользователь имеет доступ к проекту с ID {ProjectId}: {HasAccess}", projectId, hasAccess);
            return hasAccess;
        }

        public async Task<bool> IsCurrentUserManagerOrAdminWithTaskAccessAsync(int taskId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}", nameof(IsCurrentUserManagerOrAdminWithTaskAccessAsync), taskId);
            var task = await _taskRepository.FindByIdOrNullIncludeUsersAndCategoriesAsNoTrackingAsync(taskId);
            var userRole = await CurrentUserRoleInProjectOrNullAsync(task.Category.Project.Id);
            var hasAccess = userRole == UserRoles.Manager || userRole == UserRoles.Admin;
            _logger.LogInformation("Текущий пользователь имеет доступ к задаче с ID {TaskId}: {HasAccess}", taskId, hasAccess);
            return hasAccess;
        }

        public async Task<bool> IsCurrentUserManagerOrAdminWithCategoryAccessAsync(int categoryId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: categoryId = {CategoryId}", nameof(IsCurrentUserManagerOrAdminWithCategoryAccessAsync), categoryId);
            Category category = null;
            try
            {
                category = await _categoryRepository.FindByIdAsNoTrackingAsync(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при поиске категории с ID {CategoryId}", categoryId);
                return false;
            }
            var userRole = await CurrentUserRoleInProjectOrNullAsync(category.ProjectId);
            var hasAccess = userRole == UserRoles.Manager || userRole == UserRoles.Admin;
            _logger.LogInformation("Текущий пользователь имеет доступ к категории с ID {CategoryId}: {HasAccess}", categoryId, hasAccess);
            return hasAccess;
        }

        public async Task<bool> IsCurrentUserExecutorWithTaskAccessAsync(int taskId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}", nameof(IsCurrentUserExecutorWithTaskAccessAsync), taskId);
            var projectTask = await _taskRepository.FindByIdOrNullAsync(taskId);
            if (projectTask == null) 
            {
                _logger.LogWarning("Задача с ID {TaskId} не найдена.", taskId);
                return false;
            }
            var taskExecutor = projectTask.ExecutorId;
            var userId = await CurrentUserIdAsync();
            var isExecutor = userId == taskExecutor;
            _logger.LogInformation("Текущий пользователь является исполнителем задачи с ID {TaskId}: {IsExecutor}", taskId, isExecutor);
            return isExecutor;
        }

        public async Task<bool> IsCurrentUserExecutorOrManagerOrAdminWithTaskAccessAsync(int taskId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}", nameof(IsCurrentUserExecutorOrManagerOrAdminWithTaskAccessAsync), taskId);
            var isManagerOrAdmin = await IsCurrentUserManagerOrAdminWithTaskAccessAsync(taskId);
            var isExecutor = await IsCurrentUserExecutorWithTaskAccessAsync(taskId);
            var hasAccess = isExecutor || isManagerOrAdmin;
            _logger.LogInformation("Текущий пользователь имеет доступ к задаче с ID {TaskId}: {HasAccess}", taskId, hasAccess);
            return hasAccess;
        }

        public async Task<bool> IsCurrentUserExecutorOrManagerOrAdminWithProjectAccessAsync(int projectId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: projectId = {ProjectId}", nameof(IsCurrentUserExecutorOrManagerOrAdminWithProjectAccessAsync), projectId);
            var userRole = await CurrentUserRoleInProjectOrNullAsync(projectId);
            var hasAccess = userRole == UserRoles.Manager || userRole == UserRoles.Admin || userRole == UserRoles.Executor;
            _logger.LogInformation("Текущий пользователь имеет доступ к проекту с ID {ProjectId}: {HasAccess}", projectId, hasAccess);
            return hasAccess;
        }
    }
}
