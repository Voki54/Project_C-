using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface ITeamUserRepository
    {
        Task<List<Team>> GetTeamsByUser(AppUser user);
        Task<List<AppUser>> GetUsersByTeam(Team team);
        Task<TeamUser> CreateAsync(TeamUser teamUser);
        Task<TeamUser> DeleteAsync(Team team, AppUser appUser);
    }
}
