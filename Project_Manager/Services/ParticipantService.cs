using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Manager.DTO.AppUser;
using Project_Manager.Helpers;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.Services.Interfaces;

namespace Project_Manager.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IProjectUserService _projectUserService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJoinProjectService _joinProjectService;

        public ParticipantService(IProjectUserService projectUserService, UserManager<AppUser> userManager,
            IJoinProjectService joinProjectService)
        {
            _projectUserService = projectUserService;
            _userManager = userManager;
            _joinProjectService = joinProjectService;
        }

        public async Task<IEnumerable<AppUserDTO>> GetProjectParticipantsAsync(int projectId)
        {
            var participants = await _projectUserService.GetUsersFromProjectAsync(projectId);
            var admin = participants.FirstOrDefault(p => p.Role == UserRoles.Admin);

            if (admin != null)
                participants = participants.Where(p => p != admin);

            return participants;
        }

        public async Task<ParticipantControllerError.Errors?> ChangeParticipantRoleAsync(string userId, int projectId, UserRoles userRole)
        {
            if (!await _userManager.Users.AnyAsync(u => u.Id == userId))
                return ParticipantControllerError.Errors.UserNotFound;

            if (!await _projectUserService.IsUserInProjectAsync(userId, projectId))
                return ParticipantControllerError.Errors.UserNotProject;

            if (!await _projectUserService.UpdateUserRoleAsync(projectId, userId, userRole))
                return ParticipantControllerError.Errors.UpdateError;

            return null;
        }

        public async Task<ParticipantControllerError.Errors?> ExcludeParticipantAsync(string userId, int projectId)
        {
            if (!await _userManager.Users.AnyAsync(u => u.Id == userId))
                return ParticipantControllerError.Errors.UserNotFound;

            if (!await _projectUserService.IsUserInProjectAsync(userId, projectId))
                return ParticipantControllerError.Errors.UserNotProject;

            if (!await _projectUserService.ExcludeParticipantAsync(projectId, userId))
                return ParticipantControllerError.Errors.ExcludeError;
            
            await _joinProjectService.RejectApplicationAsync(userId, projectId);

            return null;
        }
    }
}
