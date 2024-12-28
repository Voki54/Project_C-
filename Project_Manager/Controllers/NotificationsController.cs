using Microsoft.AspNetCore.Mvc;
using Project_Manager.Services.Interfaces;
using Project_Manager.Helpers;
using Microsoft.AspNetCore.Authorization;


namespace Project_Manager.Controllers
{
    [Authorize]
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

        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = User.GetUserId();
            if (userId == null)
                return Unauthorized(new { message = "User is not authorized" });

            if (await _notificationService.MarkAllUserNotificationsAsReadAsync(userId))
                return Ok(new { success = true, notifications = await _notificationService.GetAvailableUserNotificationsAsync(userId) });

            return Ok(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReadNotifications()
        {
            var userId = User.GetUserId();
            if (userId == null)
                return Unauthorized(new { message = "User is not authorized" });

            if (await _notificationService.DeleteReadNotificationsAsync(userId))
                return Ok(new { success = true, notifications = await _notificationService.GetAvailableUserNotificationsAsync(userId) });

            return Ok(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteNotificationRequest request)
        {
            if (await _notificationService.DeleteAsync(request.Id))
                return Ok(new { success = true, message = "Notification was found." });

            return NotFound(new { success = false, message = "Notification not found." });
        }

        public class DeleteNotificationRequest
        {
            public int Id { get; set; }
        }
    }
}
