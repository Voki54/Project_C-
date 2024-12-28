using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Mappers;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.Services.Interfaces;
using Project_Manager.StatesManagers.Interfaces;
using Project_Manager.ViewModels;

namespace Project_Manager.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationStatesManager _notificationStatesManager;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationRepository notificationRepository, INotificationStatesManager notificationStatesManager,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _notificationStatesManager = notificationStatesManager;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(Notification notification)
        {
            if (await _notificationRepository.CreateAsync(notification) == notification)
                return true;
            return false;
        }

        public async Task<bool> DeleteAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(notificationId);

            if (notification == null)
                return false;
            return await _notificationStatesManager.ChangeNotificationState(notification, NotificationState.Deleted);
        }

        public async Task<IEnumerable<NotificationVM>> GetAvailableUserNotificationsAsync(string userId)
        {
            List<NotificationVM> notificationDTOs = new List<NotificationVM>();
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation("Invalid user ID.");
                return notificationDTOs;
            }

            var allUserNotifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
            allUserNotifications = allUserNotifications.Where
                (
                    notification => notification.State == NotificationState.Sent ||
                                    notification.State == NotificationState.Read
                ).OrderByDescending(n => n.SendDate);

            foreach (var notification in allUserNotifications)
                notificationDTOs.Add(notification.ToNotificationVM());

            return notificationDTOs;
        }

        public async Task<bool> UpdateStateAsync(Notification notification)
        {
            return await _notificationStatesManager.ChangeNotificationState(notification);
        }

        public async Task<bool> MarkAllUserNotificationsAsReadAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation("Invalid user ID.");
                return false;
            }

           return await _notificationStatesManager.ChangeStatesMultipleNotifications(userId, NotificationState.Sent);
        }

        public async Task<bool> DeleteReadNotificationsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation("Invalid user ID.");
                return false;
            }

            return await _notificationStatesManager.ChangeStatesMultipleNotifications(userId, NotificationState.Read);
        }
    }
}
