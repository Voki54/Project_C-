using Project_Manager.Models;
using Project_Manager.Services.Interfaces;

namespace Project_Manager.Services
{
    public class NotificationService : INotificationService
    {
        //private readonly IEmailService _emailService;

        public NotificationService(/*IEmailService emailService*/)
        {
            //_emailService = emailService;
        }

        public async Task SendNotificationAsync(Notification notification)
        {
            Console.WriteLine( notification.Message );
            // Отправка email (пример)
            //await _emailService.SendEmailAsync(notification.Recipient, notification.Message);
        }
    }
}
