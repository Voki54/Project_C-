using Microsoft.EntityFrameworkCore;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Repository
{
    public class TeamUserRepository : ITeamUserRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TeamUser> CreateAsync(TeamUser teamUser)
        {
            await _context.TeamsUsers.AddAsync(teamUser);
            await _context.SaveChangesAsync();
            return teamUser;
        }

        public Task<TeamUser> DeleteAsync(Team team, AppUser appUser)
        {
            throw new NotImplementedException();
        }

        public Task<List<Team>> GetTeamsByUser(AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Team>> GetTeamsByUserIdAsync(string userId)
        {
            return await _context.TeamsUsers.Where(u => u.UserId == userId)
                .Select(teamUser => new Team
                {
                    Id = teamUser.TeamId,
                    Name = teamUser.Team.Name,
                }).ToListAsync();
        }

        public async Task<UserRoles?> GetUserRoleInTeamAsync(string userId, int teamId)
        {
            var teamUser = await _context.TeamsUsers.FirstOrDefaultAsync(t => t.UserId == userId && t.TeamId == teamId);
            if (teamUser == null) return null;
            return teamUser.Role;
        }

        public Task<List<AppUser>> GetUsersByTeam(Team team)
        {
            throw new NotImplementedException();
        }
    }
}
