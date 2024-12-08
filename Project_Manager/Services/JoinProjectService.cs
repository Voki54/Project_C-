using Microsoft.AspNetCore.Identity;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Events.Notification.EventHandlers;
using Project_Manager.Events.Notification;
using Project_Manager.Models;
using Project_Manager.Services.Interfaces;
using Project_Manager.Models.Enums;
using Project_Manager.ViewModels;
using Project_Manager.DTO.JoinProject;

namespace Project_Manager.Services
{
    public class JoinProjectService : IJoinProjectService
    {
        private readonly IProjectService _projectService;
        private readonly IProjectUserService _projectUserService;
        private readonly IJoinProjectRequestRepository _joinProjectRequestRepository;
        private readonly UserManager<AppUser> _userManager;


        private readonly EventPublisher _eventPublisher;
        private readonly NotificationEventHandler _notificationHandler;

        public JoinProjectService(IProjectService projectService, IProjectUserService projectUserService,
            UserManager<AppUser> userManager, IJoinProjectRequestRepository joinProjectRequestRepository,
            EventPublisher eventPublisher, NotificationEventHandler notificationHandler)
        {
            _projectService = projectService;
            _projectUserService = projectUserService;
            _userManager = userManager;
            _joinProjectRequestRepository = joinProjectRequestRepository;

            _eventPublisher = eventPublisher;
            _notificationHandler = notificationHandler;

            // Подписка на событие
            _eventPublisher.Subscribe(async e => await HandleEventAsync(e));
        }

        // Обработчик события
        private async Task HandleEventAsync(IEvent @event)
        {
            if (@event is IEvent projectEvent)
                await _notificationHandler.HandleAsync(projectEvent);
        }

        public async Task<JoinProjectVM?> JoinProjectAsync(int projectId, string userId)
        {
            var projectName = await _projectService.GetProjectName(projectId);
            if (projectName == null)
                return null;

            JoinProjectRequestStatus? requestStatus;
            var joinProjectRequest = await _joinProjectRequestRepository.GetJoinProjectRequestAsync(projectId, userId);
            if (joinProjectRequest == null)
                requestStatus = null;
            else
                requestStatus = joinProjectRequest.Status;

            return (new JoinProjectVM(projectId, projectName, requestStatus));
        }

        public async Task SubmitJoinRequestAsync(int projectId, string userId)
        {
            await _joinProjectRequestRepository.CreateAsync(
                new JoinProjectRequest(projectId, userId, JoinProjectRequestStatus.Pending));

            var @event = new NotificationSendingEvent(userId, projectId);
            await _eventPublisher.PublishAsync(@event);
        }

        public async Task<IEnumerable<RespondDTO>> GetJoiningRequestsAsync(int projectId)
        {
            var users = await _joinProjectRequestRepository.GetUsersWithUnprocessedRequestsAsync(projectId);
            List<RespondDTO> respondDTOs = new List<RespondDTO>();

            foreach (AppUser user in users)
            {
                respondDTOs.Add(new RespondDTO(user.Id, user.Email, user.UserName, projectId));
            }

            return respondDTOs;
        }

        public async Task<bool> AcceptApplicationAsync(string userId, int projectId, UserRoles userRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            if (!await _projectUserService.IsUserInProjectAsync(userId, projectId))
            {
                await _projectUserService.AddUserToProjectAsync(projectId, userId, userRole);
            }

            await _joinProjectRequestRepository.UpdateAsync(
                new JoinProjectRequest(projectId, userId, JoinProjectRequestStatus.Accepted));
            return true;
        }

        public async Task<bool> RejectApplicationAsync(string userId, int projectId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            await _joinProjectRequestRepository.DeleteAsync(projectId, userId);
            return true;
        }
    }
}
