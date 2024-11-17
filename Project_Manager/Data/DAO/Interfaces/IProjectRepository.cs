using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync(); // TODO убрать позже
        Task<Project?> GetProjectByIdAsync(int id);
        //Task<Project> GetProjectByIdAsyncNoTracking(int id);
        //Task<IEnumerable<Project>> GetProjectById(int id);
		Task<Project> CreateAsync(Project project);
		Task<bool> DeleteAsync(int id);
		Task<bool> UpdateAsync(Project project);


		//Task<Project?> DeleteAsync(int id);
		//bool Add(Project project);
		//bool Update(Project project);
		//bool Delete(Project project);
		//bool Save();
	}
}
