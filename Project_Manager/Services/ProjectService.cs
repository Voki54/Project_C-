using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.DTO.Project;
using Project_Manager.Mappers;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.Services.Interfaces;
using Project_Manager.ViewModels;

namespace Project_Manager.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserService _projectUserService;

        public ProjectService(IProjectRepository projectRepository, IProjectUserService projectUserService)
        {
            _projectRepository = projectRepository;
            _projectUserService = projectUserService;
        }

        public async Task<IEnumerable<ProjectDTO>> GetUserProjectsAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new List<ProjectDTO>();

            var projects = await _projectUserService.GetUserProjectsAsync(userId);
            return projects.Select(p => p.ToProjectDTO()).ToList();
        }

        public async Task<ProjectDTO?> CreateProjectAsync(string? userId, CreateAndEditProjectVM createProjectVM)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var createdProject = await _projectRepository.CreateAsync(createProjectVM.ToProject());
            await _projectUserService.AddUserToProjectAsync(createdProject.Id, userId, UserRoles.Admin);
            return createdProject.ToProjectDTO();
        }

        public async Task<string?> GetProjectName(int projectId)
        {
            return await _projectRepository.GetProjectNameAsync(projectId);
        }

        public async Task<bool> ExistProjectAsync(int projectId)
        {
            return await _projectRepository.ExistProjectAsync(projectId);
        }

        public async Task<bool> UpdateProjectAsync(CreateAndEditProjectVM editProjectVM)
        {
            return await _projectRepository.UpdateAsync(
                new Project
                {
                    Id = editProjectVM.Id,
                    Name = editProjectVM.Name
                }
            );
        }

        public async Task<bool> DeleteProjectAsync(int projectId)
        {
            return await _projectRepository.DeleteAsync(projectId);
        }
    }
}
