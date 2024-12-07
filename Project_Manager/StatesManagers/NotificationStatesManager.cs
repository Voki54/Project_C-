
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.Services.Interfaces;
using Project_Manager.StatesManagers.Interfaces;

namespace Project_Manager.StatesManagers
{
    public class NotificationStatesManager : INotificationStatesManager
    {
        private readonly INotificationService _notificationService;

        public NotificationStatesManager(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<NotificationState> ChangeNotificationState(Notification notification)
        {
            switch (notification.State)
            {
                case NotificationState.Created:
                    return await HandleCreatedState(notification);

                case NotificationState.Sent:
                    return HandleSentState(notification);

/*                case NotificationState.Waiting:
                    return HandleWaitingState(notification);*/

                case NotificationState.Read:
                    return HandleReadState(notification);

                case NotificationState.Deleted:
                    throw new InvalidOperationException("No events allowed for Deleted state.");

                default:
                    throw new InvalidOperationException("Unknown state.");
            }
        }

        private async Task<NotificationState> HandleCreatedState(Notification notification)
        {
            if (notification.SendDate <= DateTime.UtcNow) 
            {
                //await _notificationService.SendNotificationAsync(notification);
                return NotificationState.Sent;
            }

            return NotificationState.Waiting;
        }

        private NotificationState HandleSentState(Notification notification)
        {
            return NotificationState.Read;
        }

        private NotificationState HandleReadState(Notification notification)
        {
            return NotificationState.Deleted;
        }
    }
}
