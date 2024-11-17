using Microsoft.EntityFrameworkCore;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Repository
{
	public class JoinProjectRequestRepository : IJoinProjectRequestRepository
	{
		private readonly ApplicationDbContext _context;

		public JoinProjectRequestRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<JoinProjectRequest> CreateAsync(JoinProjectRequest joinProjectRequest)
		{
			await _context.JoinProjectRequests.AddAsync(joinProjectRequest);
			await _context.SaveChangesAsync();
			return joinProjectRequest;
		}

		public async Task<bool> DeleteAsync(int projectId, string userId)
		{
			var joinProjectRequest = await _context.JoinProjectRequests.
				FirstOrDefaultAsync(j => j.ProjectId == projectId && j.UserId == userId);

			if (joinProjectRequest == null)
				return false;

			_context.JoinProjectRequests.Remove(joinProjectRequest);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<JoinProjectRequest?> GetJoinProjectRequestAsync(int projectId, string userId)
		{
			return await _context.JoinProjectRequests
				.FirstOrDefaultAsync(j => j.ProjectId == projectId && j.UserId == userId);
		}

		public async Task<IEnumerable<JoinProjectRequest>> GetRequestsByProjectIdAsync(int projectId)
		{
			return await _context.JoinProjectRequests.Where(j => j.ProjectId == projectId).ToListAsync();
		}
	}
}
