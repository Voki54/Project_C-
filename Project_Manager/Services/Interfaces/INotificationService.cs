using Project_Manager.DTO.Notification;
using Project_Manager.Models;

namespace Project_Manager.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> CreateAsync(Notification notification);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<NotificationDTO>> GetAvailableUserNotificationsAsync(string userId);

        Task<bool> UpdateStateAsync(Notification notification);
    }
}
