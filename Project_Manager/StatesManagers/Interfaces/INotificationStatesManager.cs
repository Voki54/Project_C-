using Microsoft.AspNetCore.Mvc;
using Project_Manager.Models;
using Project_Manager.Models.Enums;

namespace Project_Manager.StatesManagers.Interfaces
{
    public interface INotificationStatesManager
    {
        Task ChangeNotificationState(Notification notification);
    }
}


