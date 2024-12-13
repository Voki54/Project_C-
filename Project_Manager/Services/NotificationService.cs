using Project_Manager.Controllers;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.DTO.Notification;
using Project_Manager.Mappers;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.Services.Interfaces;

namespace Project_Manager.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
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
            return await _notificationRepository.DeleteAsync(notificationId);
        }

        public async Task<IEnumerable<NotificationDTO>> GetAvailableUserNotificationsAsync(string userId)
        {
            List<NotificationDTO> notificationDTOs = new List<NotificationDTO>();
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation("Invalid user ID.");
                return notificationDTOs;
            }

            var allUserNotifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
            allUserNotifications = allUserNotifications.Where
                (
                    notification => notification.State == NotificationState.Sent &&
                                    notification.State == NotificationState.Read
                );

            foreach (var notification in allUserNotifications)
                notificationDTOs.Add(notification.ToNotificationDTO());

            return notificationDTOs;
        }

        public async Task<bool> UpdateStateAsync(Notification notification)
        {
            return await _notificationRepository.UpdateStateAsync(notification);
        }
    }
}
