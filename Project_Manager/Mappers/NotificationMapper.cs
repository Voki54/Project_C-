using Project_Manager.Models;
using Project_Manager.ViewModels;

namespace Project_Manager.Mappers
{
    public static class NotificationMapper
    {
        public static NotificationVM ToNotificationVM(this Notification notification)
        {
            return new NotificationVM
            {
                Id = notification.Id,
                Message = notification.Message,
                State = notification.State,
                SendDate = notification.SendDate
            };
        }
    }
}
