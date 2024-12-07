using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(string userId);
        Task<Notification> CreateAsync(Notification notification);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateStateAsync(Notification notification);
    }
}
