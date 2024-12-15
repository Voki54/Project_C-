using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(string userId);
        Task<Notification?> GetNotificationByIdAsync(int notificationId);
        Task<Notification> CreateAsync(Notification notification);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateStateAsync(Notification notification);
        Task<int> MarkAllUserNotificationsAsReadAsync(string userId);
        Task<int> DeleteReadNotificationsAsync(string userId);
    }
}
