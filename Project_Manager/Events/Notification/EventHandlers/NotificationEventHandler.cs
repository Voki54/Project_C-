using Project_Manager.Models;
using Project_Manager.Services.Interfaces;

namespace Project_Manager.Events.Notification.EventHandlers
{
    public class NotificationEventHandler
    {
        private readonly INotificationService _notificationService;

        public NotificationEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Асинхронный метод для обработки события
        public async Task HandleAsync(ProjectApplicationSubmittedEvent @event)
        {
            var notification = new Models.Notification
            {
                Message = $"New application received: {@event.ApplicantName} applied for project {@event.ProjectName}.",
                RecipientId = "Admin",
                SendDate = @event.Timestamp
            };

            // Асинхронная отправка уведомления (например, email)
            await _notificationService.SendNotificationAsync(notification);
        }
    }
}
