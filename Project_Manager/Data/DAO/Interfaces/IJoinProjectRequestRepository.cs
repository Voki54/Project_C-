using Project_Manager.Models.Enum;
using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Interfaces
{
	public interface IJoinProjectRequestRepository
	{
		Task<IEnumerable<JoinProjectRequest>> GetRequestsByProjectIdAsync(int projectId);
        Task<IEnumerable<string>> GetUsersIdWithUnprocessedRequestsAsync(int projectId);
        Task<JoinProjectRequest?> GetJoinProjectRequestAsync(int projectId, string userId);

		Task<JoinProjectRequest> CreateAsync(JoinProjectRequest joinProjectRequest);
        Task<bool> UpdateAsync(JoinProjectRequest joinProjectRequest);
        Task<bool> DeleteAsync(int projectId, string userId);
	}
}
