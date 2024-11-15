using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetAllAsync(); // TODO убрать позже
        Task<Team?> GetTeamByIdAsync(int id);
        //Task<Team> GetTeamByIdAsyncNoTracking(int id);
        //Task<IEnumerable<Team>> GetTeamById(int id);
		Task<Team> CreateAsync(Team team);
		Task<bool> DeleteAsync(int id);
		Task<bool> UpdateAsync(Team team);


		//Task<Team?> DeleteAsync(int id);
		//bool Add(Team team);
		//bool Update(Team team);
		//bool Delete(Team team);
		//bool Save();
	}
}
