using Project_Manager.Models.Enums;

namespace Project_Manager.Services.Interfaces
{
    public interface IUserAccessService
    {
        Task<string?> CurrentUserIdAsync();
        Task<UserRoles?> CurrentUserRoleInProjectOrNullAsync(int projectId);
        Task<UserRoles?> CurrentUserRoleInProjectOrTaskExecutorOrNullAsync(int taskId, int projectId);
        Task<bool> IsCurrentUserManagerOrAdminWithProjectAccessAsync(int projectId);
        Task<bool> IsCurrentUserManagerOrAdminWithTaskAccessAsync(int taskId);
        Task<bool> IsCurrentUserManagerOrAdminWithCategoryAccessAsync(int categoryId);
        Task<bool> IsCurrentUserExecutorWithTaskAccessAsync(int taskId);
        Task<bool> IsCurrentUserExecutorOrManagerOrAdminWithTaskAccessAsync(int taskId);
        Task<bool> IsCurrentUserExecutorOrManagerOrAdminWithProjectAccessAsync(int projectId);
    }
}
