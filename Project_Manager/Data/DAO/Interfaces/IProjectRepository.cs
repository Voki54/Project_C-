using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project?> GetProjectByIdAsync(int id);
        Task<string?> GetProjectNameAsync(int id);
        Task<Project> CreateAsync(Project project);
		Task<bool> DeleteAsync(int id);
        Task<bool> ExistProjectAsync(int projectId);
        Task<bool> UpdateAsync(Project project);
	}
}
