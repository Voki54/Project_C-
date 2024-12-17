using Microsoft.AspNetCore.Mvc;
using Project_Manager.Helpers;
using Project_Manager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Project_Manager.Models.Enums;
using Project_Manager.ViewModels;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class JoinProjectController : Controller
    {
        private readonly IJoinProjectService _joinProjectService;

        public JoinProjectController(IJoinProjectService joinProjectService)
        {
            _joinProjectService = joinProjectService;
        }

        [HttpGet]
        public async Task<IActionResult> Join(int? projectId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            JoinProjectVM? joinProjectVM = null;

            if (projectId == null)
                return View(joinProjectVM);

            joinProjectVM = await _joinProjectService.JoinProjectAsync(projectId!.Value, userId);
            if (joinProjectVM != null)
                return View(joinProjectVM);

            TempData["ErrorMessage"] = "Ошибка при подачи заявки. Убедитесь, что вы используете правильную ссылку-приглашение.";
            return RedirectToAction("Index", "Error");
        }

        [HttpPost("JoinProject/SubmitJoinWithoutUri")]
        public async Task<IActionResult> SubmitJoinRequest(int projectId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            if (await _joinProjectService.SubmitJoinRequestAsync(projectId, userId))
                return RedirectToAction("Join", "JoinProject", new { projectId });

            TempData["ErrorMessage"] = "Ошибка при подачи заявки. Убедитесь, что вы используете правильную ссылку-приглашение.";
            return RedirectToAction("Index", "Error");
        }

        [HttpPost("JoinProject/SubmitJoinWithUri")]
        public async Task<IActionResult> SubmitJoinRequest(string projectLink)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var projectId = await _joinProjectService.SubmitJoinRequestAsync(projectLink, userId);

            if (projectId != null)
                return RedirectToAction("Join", "JoinProject", new { projectId });

            TempData["ErrorMessage"] = "Ошибка при подачи заявки. Убедитесь, что вы используете правильную ссылку-приглашение.";
            return RedirectToAction("Index", "Error");
        }

        [HttpGet]
        public async Task<IActionResult> Respond(int projectId)
        {
            return View(await _joinProjectService.GetJoiningRequestsAsync(projectId));
        }

        [HttpPost]
        public async Task<IActionResult> AcceptApplication(string userId, int projectId, UserRoles userRole)
        {
            if (await _joinProjectService.AcceptApplicationAsync(userId, projectId, userRole))
                return RedirectToAction("Respond", new { projectId });

            TempData["ErrorMessage"] = "Что-то пошло не так при подтверждении заявки.";
            return RedirectToAction("Index", "Error");
        }

        [HttpPost]
        public async Task<IActionResult> RejectApplication(string userId, int projectId)
        {
            if (await _joinProjectService.RejectApplicationAsync(userId, projectId))
                return RedirectToAction("Respond", new { projectId });

            TempData["ErrorMessage"] = "Что-то пошло не так при отклонении заявки.";
            return RedirectToAction("Index", "Error");
        }
    }
}
