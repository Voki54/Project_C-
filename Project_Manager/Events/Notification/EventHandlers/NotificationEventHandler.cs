using Microsoft.AspNetCore.Identity;
using Project_Manager.Models;
using Project_Manager.Services;
using Project_Manager.Services.Interfaces;
using Project_Manager.StatesManagers.Interfaces;

namespace Project_Manager.Events.Notification.EventHandlers
{
    public class NotificationEventHandler
    {
        private readonly INotificationStatesManager _notificationStateManager;
        private readonly INotificationService _notificationService;
        private readonly IProjectUserService _projectUserService;
        private readonly IProjectService _projectService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<NotificationService> _logger;

        public NotificationEventHandler(INotificationStatesManager notificationStateManager, 
            INotificationService notificationService, IProjectUserService projectUserService,
            UserManager<AppUser> userManager, IProjectService projectService,
            ILogger<NotificationService> logger)
        {
            _notificationStateManager = notificationStateManager;
            _notificationService = notificationService;
            _projectUserService = projectUserService;
            _userManager = userManager;
            _projectService = projectService;
            _logger = logger;
        }

        public async Task HandleAsync(INotificationEvent notificationEvent)
        {
            if (!await _projectService.ExistProjectAsync(notificationEvent.ProjectId))
            {
                _logger.LogInformation("Invalid project ID.");
                return;
            }

            Models.Notification? notification = null;

            switch (notificationEvent.Type)
            {
                case NotificationType.JoinProject:
                    notification = await HandleJoinProjectEvent(notificationEvent);
                    break;
                case NotificationType.AcceptJoin:
                    notification = await HandleAcceptJoinEvent(notificationEvent);
                    break;
                default:
                    throw new InvalidOperationException("Unknown notification event type.");
            }

            if (notification != null && await _notificationService.CreateAsync(notification))
                await _notificationStateManager.ChangeNotificationState(notification);
        }

        private async Task<Models.Notification?> HandleJoinProjectEvent(INotificationEvent notificationEvent)
        {
            if (notificationEvent.SenderId == null)
                return null;

            var user = await _userManager.FindByIdAsync(notificationEvent.SenderId);

            if (user == null)
            {
                _logger.LogInformation("Invalid sender ID.");
                return null;
            }

            var adminId = await _projectUserService.GetAdminIdAsync(notificationEvent.ProjectId);

            if (adminId == null)
            {
                _logger.LogInformation("Invalid admin ID.");
                return null;
            }

            string projectName = await _projectService.GetProjectName(notificationEvent.ProjectId);

            return new Models.Notification
            {
                Message = $"Пользователь {user.UserName} хочет присоединиться к проекту \"{projectName}\".",
                RecipientId = adminId,
                SendDate = notificationEvent.Timestamp
            };
        }

        private async Task<Models.Notification?> HandleAcceptJoinEvent(INotificationEvent notificationEvent)
        {
            if (notificationEvent.RecipientId == null)
                return null;

            if (await _userManager.FindByIdAsync(notificationEvent.RecipientId) == null)
            {
                _logger.LogInformation("Invalid recipient ID.");
                return null;
            }

            string projectName = await _projectService.GetProjectName(notificationEvent.ProjectId);

            return new Models.Notification
            {
                Message = $"Вы были добавлены в проект \"{projectName}\".",
                RecipientId = notificationEvent.RecipientId,
                SendDate = notificationEvent.Timestamp
            };
        }
    }
}
