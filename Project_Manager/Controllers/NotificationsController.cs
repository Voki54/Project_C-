using Microsoft.AspNetCore.Mvc;
using Project_Manager.Services.Interfaces;
using Project_Manager.Helpers;
using Azure.Core;
using Project_Manager.Models;


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

        /*        [HttpPost]
                public async Task<IActionResult> MarkAsRead()
                {

                }*/

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteNotificationRequest request)
        {
            if (await _notificationService.DeleteAsync(request.Id))
                return Ok();

            return NotFound(new { Message = "Notification not found" });
        }

        public class DeleteNotificationRequest
        {
            public int Id { get; set; }
        }
    }
}
