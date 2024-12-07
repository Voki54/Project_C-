using Project_Manager.Models;

namespace Project_Manager.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(Notification notification);
    }
}
