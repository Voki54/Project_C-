using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface IProjectUserRepository
    {
        Task<IEnumerable<Project>> GetProjectsByUserIdAsync(string userId);
        Task<UserRoles?> GetUserRoleInProjectAsync(string userId, int projectId);
        Task<List<AppUser>> GetUsersByProject(Project project);
        Task<ProjectUser> CreateAsync(ProjectUser projectUser);
        Task<ProjectUser> DeleteAsync(Project project, AppUser appUser);
    }
}
