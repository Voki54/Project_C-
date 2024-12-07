using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.Services.Interfaces;
using Project_Manager.StatesManagers;
using Project_Manager.StatesManagers.Interfaces;

namespace Project_Manager.Events.Notification.EventHandlers
{
    public class NotificationEventHandler
    {
        private readonly INotificationStatesManager _notificationStateManager;
        private readonly INotificationService _notificationService;
        private readonly IProjectUserService _projectUserService;

        public NotificationEventHandler(INotificationStatesManager notificationStateManager, 
            INotificationService notificationService, IProjectUserService projectUserService)
        {
            _notificationStateManager = notificationStateManager;
            _notificationService = notificationService;
            _projectUserService = projectUserService;
        }

        // Асинхронный метод для обработки события
        public async Task HandleAsync(IEvent @event)
        {
            var adminId = await _projectUserService.GetAdminId(@event.ProjectId);
            //тут должен быть лог
            if (adminId == null) return;
            var notification = new Models.Notification
            {
                Message = $"New application received: {@event.SenderId} applied for project {@event.ProjectId}.",
                RecipientId = adminId.ToString(),
                SendDate = @event.Timestamp
            };

            if (await _notificationService.CreateAsync(notification))
                await _notificationStateManager.ChangeNotificationState(notification);


            //await notification.HandleEventAsync(@event);

            // Асинхронная отправка уведомления (например, email)
            //await _notificationService.SendNotificationAsync(notification);
        }
    }
}
