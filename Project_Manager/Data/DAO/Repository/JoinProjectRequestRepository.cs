using Microsoft.EntityFrameworkCore;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;
using Project_Manager.Models.Enum;

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

		public async Task<IEnumerable<string>> GetUsersIdWithUnprocessedRequestsAsync(int projectId)
		{
			return await _context.JoinProjectRequests
				.Where(j => j.ProjectId == projectId && j.Status == JoinProjectRequestStatus.Pending)
				.Select(joinProjectRequest => joinProjectRequest.UserId)
				.ToListAsync();
        }

        public async Task<bool> UpdateAsync(JoinProjectRequest joinProjectRequest)
        {
            var existingJoinProjectRequest = await _context.JoinProjectRequests.FindAsync(joinProjectRequest.ProjectId, joinProjectRequest.UserId);

            if (existingJoinProjectRequest == null)
            {
                return false;
            }

            existingJoinProjectRequest.Status = joinProjectRequest.Status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
