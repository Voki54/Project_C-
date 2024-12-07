using Microsoft.AspNetCore.Mvc;

namespace Project_Manager.Controllers
{
    public class NotificationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
