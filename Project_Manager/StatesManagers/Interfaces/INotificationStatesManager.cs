using Project_Manager.Models;
using Project_Manager.Models.Enums;

namespace Project_Manager.StatesManagers.Interfaces
{
    public interface INotificationStatesManager
    {
        Task<NotificationState> ChangeNotificationState(Notification notification);
    }
}


