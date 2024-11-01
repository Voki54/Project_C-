using Microsoft.EntityFrameworkCore;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Repository
{
	public class TeamRepository : ITeamRepository
	{
		private readonly ApplicationDbContext _context;

		public TeamRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		//TODO добавить возможность обработки запросов
		public async Task<IEnumerable<Team>> GetAllAsync()
		{
			return await _context.Teams.ToListAsync();
		}

		public async Task<Team?> GetTeamByIdAsync(int id)
		{
			return await _context.Teams.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<Team> CreateAsync(Team team)
		{
			await _context.Teams.AddAsync(team);
			await _context.SaveChangesAsync();
			return team;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);

			if (team == null)
				return false;

			_context.Teams.Remove(team);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> UpdateAsync(int id, Team team)
		{
			var existingTeam = await _context.Teams.FindAsync(id);

			if (existingTeam == null)
			{
				return false;
			}

			existingTeam.Name = team.Name;
			await _context.SaveChangesAsync();
			return true;
		}
	}
}

