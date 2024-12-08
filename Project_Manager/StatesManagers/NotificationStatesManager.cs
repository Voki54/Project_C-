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

        public async Task ChangeNotificationState(Notification notification)
        {
            switch (notification.State)
            {
                case NotificationState.Created:
                    await HandleCreatedState(notification);
                    break;

                case NotificationState.Sent:
                    await HandleSentState(notification);
                    break;

                case NotificationState.Read:
                    await HandleReadState(notification);
                    break;

                default:
                    throw new InvalidOperationException("Unknown notification state.");
            }

            /*            (notification.State) switch
                        {
                            NotificationState.Created => await HandleCreatedState(notification),
                            NotificationState.Sent => HandleSentState(notification),
                            NotificationState.Read => HandleReadState(notification),
                            _ => throw new InvalidOperationException("Unknown notification state.")*/
            /*NotificationState.Created =>
                return await HandleCreatedState(notification);

            case NotificationState.Sent:
                return HandleSentState(notification);

*//*                case NotificationState.Waiting:
                    return HandleWaitingState(notification);*//*

                case NotificationState.Read:
                    return HandleReadState(notification);

                    // убрать вообще это состояние
                //case NotificationState.Deleted:
                //    throw new InvalidOperationException("No events allowed for Deleted state.");

                default:
                    throw new InvalidOperationException("Unknown state notitfication.");*/
        }

        private async Task HandleCreatedState(Notification notification)
        {
            if (notification.SendDate <= DateTime.UtcNow)
                notification.State = NotificationState.Sent;
            else
                notification.State = NotificationState.Waiting;
            await _notificationService.UpdateStateAsync(notification);
        }

        private async Task HandleSentState(Notification notification)
        {
            notification.State = NotificationState.Read;
            await _notificationService.UpdateStateAsync(notification);
        }

        private async Task HandleReadState(Notification notification)
        {
            await _notificationService.DeleteAsync(notification.Id);
        }
    }
}
