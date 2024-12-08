using Microsoft.AspNetCore.Mvc;
using Project_Manager.Helpers;
using Project_Manager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Project_Manager.Models.Enums;

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
        public async Task<IActionResult> Join(int projectId)
        {
            var userId = User.GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var joinProjectVM = await _joinProjectService.JoinProjectAsync(projectId, userId);
            if (joinProjectVM == null)
            {
                TempData["ErrorMessage"] = "Ошибка при подачи заявки. Убедитесь, что вы используете правильную ссылку-приглашение.";
                return RedirectToAction("Index", "Error");
            }


            return View(joinProjectVM);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitJoinRequest(int projectId)
        {
            var userId = User.GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            await _joinProjectService.SubmitJoinRequestAsync(projectId, userId);

            return RedirectToAction("Join", "JoinProject", new { projectId });
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
