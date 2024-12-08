using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.DTO.AppUser;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.Services.Interfaces;

namespace Project_Manager.Services
{
    public class ProjectUserService : IProjectUserService
    {
        private readonly IProjectUserRepository _projectUserRepository;

        public ProjectUserService(IProjectUserRepository projectUserRepository)
        {
            _projectUserRepository = projectUserRepository;
        }

        public async Task<IEnumerable<Project>> GetUserProjectsAsync(string userId)
        {
            return await _projectUserRepository.GetProjectsByUserIdAsync(userId);
        }

        public async Task AddUserToProjectAsync(int projectId, string userId, UserRoles userRole)
        {
            await _projectUserRepository.CreateAsync(
                new ProjectUser
                    {
                        ProjectId = projectId,
                        UserId = userId,
                        Role = userRole
                    }
                );
        }

        public async Task<bool> UpdateUserRoleAsync(int projectId, string userId, UserRoles userRole)
        {
            return await _projectUserRepository.UpdateAsync(
                new ProjectUser
                {           
                    ProjectId = projectId,
                    UserId = userId,
                    Role = userRole
                }
            );
        }

        public async Task<bool> ExcludeParticipantAsync(int projectId, string userId)
        {
            return await _projectUserRepository.DeleteAsync(projectId, userId);
        }

        public async Task<IEnumerable<AppUserDTO>> GetUserFromProjectAsync(int projectId)
        {
            return await _projectUserRepository.GetUsersByProjectIdAsync(projectId);
        }

        public async Task<string?> GetAdminId(int projectId)
        {
            return await _projectUserRepository.GetAdminIdInProjectAsync(projectId);
        }

        public async Task<bool> IsUserInProjectAsync(string userId, int projectId)
        {
            return await _projectUserRepository.IsUserInProjectAsync(userId, projectId);
        }
    }

}
