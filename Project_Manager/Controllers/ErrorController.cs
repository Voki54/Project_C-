using Microsoft.AspNetCore.Mvc;

namespace Project_Manager.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }
    }
}
