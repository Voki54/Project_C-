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

        public Task<List<AppUser>> GetUsersByTeam(Team team)
        {
            throw new NotImplementedException();
        }
    }
}
