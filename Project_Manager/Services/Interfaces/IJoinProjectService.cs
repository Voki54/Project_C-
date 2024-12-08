using Project_Manager.DTO.JoinProject;
using Project_Manager.Models.Enums;
using Project_Manager.ViewModels;

namespace Project_Manager.Services.Interfaces
{
    public interface IJoinProjectService
    {
        Task<JoinProjectVM?> JoinProjectAsync(int projectId, string userId);
        Task SubmitJoinRequestAsync(int projectId, string userId);
        Task<IEnumerable<RespondDTO>> GetJoiningRequestsAsync(int projectId);
        Task<bool> AcceptApplicationAsync(string userId, int projectId, UserRoles userRole);
        Task<bool> RejectApplicationAsync(string userId, int projectId);
    }
}
