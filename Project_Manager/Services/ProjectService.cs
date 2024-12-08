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
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly IProjectUserService _projectUserService;

        public ProjectService(IProjectRepository projectRepository, IProjectUserRepository projectUserRepository,
            IProjectUserService projectUserService)
        {
            _projectRepository = projectRepository;
            _projectUserRepository = projectUserRepository;
            _projectUserService = projectUserService;
        }

        public async Task<IEnumerable<ProjectDTO>> GetUserProjectsAsync(string? userId)
        {
            if (userId == null)
                return new List<ProjectDTO>();

            var projects = await _projectUserService.GetUserProjectsAsync(userId);
            return projects.Select(p => p.ToProjectDTO()).ToList();
        }

        public async Task<ProjectDTO?> CreateProjectAsync(string? userId, CreateAndEditProjectVM createProjectVM)
        {
            if (userId == null)
                return null;

            var createdProject = await _projectRepository.CreateAsync(createProjectVM.ToProject());
            await _projectUserService.AddUserToProjectAsync(createdProject.Id, userId, UserRoles.Admin);
            return createdProject.ToProjectDTO();
        }

/*        public async Task<CreateAndEditProjectVM?> GetCreateAndEditProjectVMByIdAsync(int projectId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);

            if (project == null)
                return null;

            return project.ToCreateAndEditProjectVM();
        }*/

        public async Task<string?> GetProjectName(int projectId)
        {
            return await _projectRepository.GetProjectName(projectId);
        }

        public async Task<bool> ExistProjectAsync(int projectId)
        {
            return await _projectRepository.ExistProjectAsync(projectId);
        }

        public async Task<bool> UpdateProjectAsync(int projectId, CreateAndEditProjectVM editProjectVM)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            
            if (project == null)
                return false;

            bool updateResult = await _projectRepository.UpdateAsync(
                new Project
                {
                    Id = projectId,
                    Name = editProjectVM.Name
                }
            );

            if (!updateResult)
                return false;

            return true;
        }
    }
}
