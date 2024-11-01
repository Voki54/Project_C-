using Project_Manager.Data;
using Project_Manager.Models;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data.DAO.Interfaces;

namespace Project_Manager.Data.DAO.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;
        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Team team)
        {
            _context.Add(team);
            return Save();
        }

        public bool Delete(Team team)
        {
            _context.Remove(team);
            return Save();
        }

        public async Task<IEnumerable<Team>> GetAll()
        {
            return await _context.Teams.ToListAsync();
        }

        public Task<Team> GetTeamByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Team> GetTeamByIdAsyncNoTracking(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Team>> GetTeamsByGroupId(int id)
        {
            throw new NotImplementedException();
        }

        //public async Task<Team> GetTeamByIdAsync(int id)
        //{
        //	return await _context.Teams.FirstOrDefaultAsync(i => i.Id == id);
        //}

        //public async Task<Team> GetTeamByIdAsyncNoTracking(int id)
        //{
        //	return await _context.Teams.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        //}

        //public async Task<IEnumerable<Team>> GetTeamsByUserId(int id)
        //{
        //	var result = from team in _context.Teams
        //				 where team.GroupId == id
        //				 select team;
        //	return result;
        //}

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Team team)
        {
            _context.Update(team);
            return Save();
        }
    }
}

