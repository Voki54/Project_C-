using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.StatesManagers.Interfaces;

namespace Project_Manager.StatesManagers
{
    public class NotificationStatesManager : INotificationStatesManager
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationStatesManager(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> ChangeNotificationState(Notification notification, NotificationState? nextState)
        {
            switch (notification.State)
            {
                case NotificationState.Created:
                    return await HandleCreatedState(notification);

                case NotificationState.Sent:
                    if (nextState == NotificationState.Deleted)
                        return await HandleReadState(notification);
                    return await HandleSentState(notification);

                case NotificationState.Read:
                    return await HandleReadState(notification);

                default:
                    throw new InvalidOperationException("Unknown notification state.");
            }
        }

        public async Task<bool> ChangeStatesMultipleNotifications(string recipientId, NotificationState currentState)
        {
            switch (currentState)
            {
                case NotificationState.Sent:
                    return await HandleSentState(recipientId);

                case NotificationState.Read:
                    return await HandleReadState(recipientId);

                default:
                    throw new InvalidOperationException("Unknown notification state.");
            }
        }

        private async Task<bool> HandleCreatedState(Notification notification)
        {
            if (notification.SendDate <= DateTime.Now)
                notification.State = NotificationState.Sent;
            else
                notification.State = NotificationState.Waiting;
            return await _notificationRepository.UpdateStateAsync(notification);
        }

        private async Task<bool> HandleSentState(Notification notification)
        {
            notification.State = NotificationState.Read;
            return await _notificationRepository.UpdateStateAsync(notification);
        }

        private async Task<bool> HandleSentState(string recipientId)
        {
            if (await _notificationRepository.MarkAllUserNotificationsAsReadAsync(recipientId) > 0)
                return true;
            return false;
        }

        private async Task<bool> HandleReadState(Notification notification)
        {
           return await _notificationRepository.DeleteAsync(notification.Id);
        }

        private async Task<bool> HandleReadState(string recipientId)
        {
            if (await _notificationRepository.DeleteReadNotificationsAsync(recipientId) > 0)
                return true;
            return false;
        }
    }
}
