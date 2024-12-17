using Project_Manager.Models;
using Project_Manager.Models.Enums;

namespace Project_Manager.StatesManagers.Interfaces
{
    public interface INotificationStatesManager
    {
        Task<bool> ChangeNotificationState(Notification notification, NotificationState? nextState = null);
        Task<bool> ChangeStatusMultipleNotifications(string recipientId, NotificationState notificationState);
    }
}
