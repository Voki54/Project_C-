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

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> CreateAsync(Notification notification)
        {
            if (await _notificationRepository.CreateAsync(notification) == notification)
                return true;
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (await _notificationRepository.DeleteAsync(id))
                return true;
            return false;
        }

        public async Task<IEnumerable<NotificationDTO>> GetAvailableUserNotificationsAsync(string userId)
        {
            List<NotificationDTO> notificationDTOs = new List<NotificationDTO>();
            if (string.IsNullOrEmpty(userId))
                //тут должен быть лог
                return notificationDTOs;

            var allUserNotifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
            allUserNotifications = allUserNotifications.Where
                (
                    notification => notification.State != NotificationState.Created &&
                                    notification.State != NotificationState.Waiting
                );

            foreach (var notification in allUserNotifications)
                notificationDTOs.Add(notification.ToNotificationDTO());
            return notificationDTOs;
        }

        public async Task<bool> UpdateStateAsync(Notification notification)
        {
            if (await _notificationRepository.UpdateStateAsync(notification))
                return true;
            return false;
        }

        /*        public async Task SendNotificationAsync(Notification notification)
                {
                    Console.WriteLine( notification.Message );
                    // Отправка email (пример)
                    //await _emailService.SendEmailAsync(notification.Recipient, notification.Message);
                }*/
    }
}
