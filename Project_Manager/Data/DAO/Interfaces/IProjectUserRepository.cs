using Project_Manager.DTO.AppUser;
using Project_Manager.Models;
using Project_Manager.Models.Enums;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface IProjectUserRepository
    {
        Task<IEnumerable<Project>> GetProjectsByUserIdAsync(string userId);
        Task<IEnumerable<AppUserDTO>> GetUsersByProjectIdAsync(int projectId);
        Task<UserRoles?> GetUserRoleInProjectAsync(string userId, int projectId);
        Task<bool> IsUserInProjectAsync(string userId, int projectId);
        Task<ProjectUser> CreateAsync(ProjectUser projectUser);
        Task<bool> UpdateAsync(ProjectUser projectUser);
        Task<bool> DeleteAsync(int projectId, string userId);

    }
}
