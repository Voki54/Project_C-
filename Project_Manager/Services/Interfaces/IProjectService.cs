using Project_Manager.DTO.Project;
using Project_Manager.ViewModels;

namespace Project_Manager.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDTO>> GetUserProjectsAsync(string? userId);
        Task<ProjectDTO?> CreateProjectAsync(string? userId, CreateAndEditProjectVM createProjectVM);
        //Task<CreateAndEditProjectVM?> GetCreateAndEditProjectVMByIdAsync(int projectId);
        Task<string?> GetProjectName(int projectId);
        Task<bool> ExistProjectAsync(int projectId);
        Task<bool> UpdateProjectAsync(int projectId, CreateAndEditProjectVM editProjectVM);
    }
}
