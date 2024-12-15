using Mono.TextTemplating;
using Project_Manager.DTO.Notification;
using Project_Manager.Models;

namespace Project_Manager.Mappers
{
    public static class NotificationMapper
    {
        public static NotificationDTO ToNotificationDTO(this Notification notification)
        {
            return new NotificationDTO
            {
                Id = notification.Id,
                Message = notification.Message,
                State = notification.State,
                SendDate = notification.SendDate
            };
        }
    }
}
