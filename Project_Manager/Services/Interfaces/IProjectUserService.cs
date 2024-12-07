using Project_Manager.DTO.AppUser;
using Project_Manager.Models.Enums;
using Project_Manager.Models;

namespace Project_Manager.Services.Interfaces
{
    public interface IProjectUserService
    {
        Task<IEnumerable<Project>> GetUserProjectsAsync(string userId);

        Task<bool> AddUserToProjectAsync(int projectId, string userId, UserRoles userRole);

        Task<bool> UpdateUserRoleAsync(int projectId, string userId, UserRoles userRole);

        Task<bool> ExcludeParticipantAsync(int projectId, string userId);

        Task<IEnumerable<AppUserDTO>> GetUserFromProjectAsync(int projectId);
        Task<string?> GetAdminId(int projectId);
    }
}
