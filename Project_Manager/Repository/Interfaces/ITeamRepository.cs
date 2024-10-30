using Project_Manager.Models;

namespace Project_Manager.Interfaces
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetAll();
        Task<Team> GetTeamByIdAsync(int id);
        Task<Team> GetTeamByIdAsyncNoTracking(int id);
		Task<IEnumerable<Team>> GetTeamsByGroupId(int id);
		bool Add(Team team);
        bool Update(Team team);
        bool Delete(Team team);
        bool Save();
    }
}
