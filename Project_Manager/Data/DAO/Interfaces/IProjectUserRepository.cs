using Project_Manager.Models;
using Project_Manager.Models.Enum;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface IProjectUserRepository
    {
        Task<IEnumerable<Project>> GetProjectsByUserIdAsync(string userId);
        Task<UserRoles?> GetUserRoleInProjectAsync(string userId, int projectId);
        Task<bool> IsUserInProjectAsync(string userId, int projectId);
        Task<ProjectUser> CreateAsync(ProjectUser projectUser);
        Task<bool> DeleteAsync(int projectId, string userId);
    }
}
