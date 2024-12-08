using Project_Manager.DTO.AppUser;
using Project_Manager.Helpers;
using Project_Manager.Models.Enums;

namespace Project_Manager.Services.Interfaces
{
    public interface IParticipantService
    {
        Task<IEnumerable<AppUserDTO>> GetProjectParticipantsAsync(int projectId);
        Task<ParticipantControllerError?> ChangeParticipantRoleAsync(string userId, int projectId, UserRoles userRole);
    }
}
