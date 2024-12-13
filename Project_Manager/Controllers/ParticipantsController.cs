using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Helpers;
using Project_Manager.Models.Enums;
using Project_Manager.Services.Interfaces;
using Project_Manager.ViewModels;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class ParticipantsController : Controller
    {
        private readonly IParticipantService _participantService;

        public ParticipantsController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int projectId)
        {
            return View(new ParticipantsVM(projectId, await _participantService.GetProjectParticipantsAsync(projectId)));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, int projectId, UserRoles userRole)
        {
            var error = await _participantService.ChangeParticipantRoleAsync(userId, projectId, userRole);

            if (!error.HasValue)
                return RedirectToAction("Index", new { projectId });

            TempData["ErrorMessage"] = ParticipantControllerError.errorsDescription[error!.Value];
            return RedirectToAction("Index", "Error");
        }

        [HttpPost]
        public async Task<IActionResult> ExcludeParticipant(string userId, int projectId)
        {
            var error = await _participantService.ExcludeParticipantAsync(userId, projectId);

            if (!error.HasValue)
                return RedirectToAction("Index", new { projectId });

            TempData["ErrorMessage"] = ParticipantControllerError.errorsDescription[error!.Value];
            return RedirectToAction("Index", "Error");
        }
    }
}
