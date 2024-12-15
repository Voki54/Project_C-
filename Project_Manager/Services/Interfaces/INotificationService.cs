using Project_Manager.DTO.Notification;
using Project_Manager.Models;
using Project_Manager.Models.Enums;

namespace Project_Manager.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> CreateAsync(Notification notification);
        Task<bool> DeleteAsync(int notificationId);
        Task<IEnumerable<NotificationDTO>> GetAvailableUserNotificationsAsync(string userId);
        Task<bool> UpdateStateAsync(Notification notification);
        Task<bool> MarkAllUserNotificationsAsReadAsync(string userId);
        Task<bool> DeleteReadNotificationsAsync(string userId);
    }
}
