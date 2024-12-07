using Microsoft.AspNetCore.Mvc;
using Project_Manager.Services.Interfaces;
using Project_Manager.Helpers;


namespace Project_Manager.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _notificationService.GetAvailableUserNotificationsAsync(User.GetUserId()));
        }
    }
}
