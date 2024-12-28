using Project_Manager.Models;
using Project_Manager.Models.Enums;

namespace Project_Manager.StatesManagers.Interfaces
{
    public interface INotificationStatesManager
    {
        Task<bool> ChangeNotificationState(Notification notification, NotificationState? nextState = null);
        Task<bool> ChangeStatesMultipleNotifications(string recipientId, NotificationState notificationState);
    }
}
