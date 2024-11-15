using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface ITeamUserRepository
    {
        Task<IEnumerable<Team>> GetTeamsByUserIdAsync(string userId);
        Task<UserRoles?> GetUserRoleInTeamAsync(string userId, int teamId);
        Task<List<AppUser>> GetUsersByTeam(Team team);
        Task<TeamUser> CreateAsync(TeamUser teamUser);
        Task<TeamUser> DeleteAsync(Team team, AppUser appUser);
    }
}
