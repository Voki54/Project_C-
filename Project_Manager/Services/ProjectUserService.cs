using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;
using Project_Manager.Models.Enums;

namespace Project_Manager.Services
{
    public class ProjectUserService
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

        public async Task<bool> AddUserToProjectAsync(int projectId, string userId, UserRoles userRole)
        {
            await _projectUserRepository.CreateAsync(
                new ProjectUser
                {
                    ProjectId = projectId,
                    UserId = userId,
                    Role = userRole
                }
                );
            return true;
        }
    }

}
