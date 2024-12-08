using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Helpers;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.Services;
using Project_Manager.Services.Interfaces;
using Project_Manager.ViewModels;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class ParticipantsController : Controller
    {
        private readonly IParticipantService _participantService;
        //private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJoinProjectRequestRepository _joinProjectRequestRepository;
        private readonly IProjectUserService _projectUserService;

        public ParticipantsController(IParticipantService participantService, IProjectUserRepository projectUserRepository,
            UserManager<AppUser> userManager, IJoinProjectRequestRepository joinProjectRequestRepository, IProjectUserService projectUserService)
        {
            _participantService = participantService;
            _projectUserRepository = projectUserRepository;
            _userManager = userManager;
            _joinProjectRequestRepository = joinProjectRequestRepository;
            _projectUserService = projectUserService;
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

            switch (error)
            {
                case ParticipantControllerError.UserNotFound:
                    TempData["ErrorMessage"] = "Не найден пользователь для изменения роли.";
                    return RedirectToAction("Index", "Error");
                case ParticipantControllerError.UserNotProject:
                    TempData["ErrorMessage"] = "Пользователь не состоит в проекте.";
                    return RedirectToAction("Index", "Error");
                case ParticipantControllerError.UpdateError:
                    TempData["ErrorMessage"] = "Ошибка изменения роли пользователя.";
                    return RedirectToAction("Index", "Error");
            }

            return RedirectToAction("Index", new { projectId });
        }

        [HttpPost]
        public async Task<IActionResult> ExcludeParticipant(string userId, int projectId)
        {
/*            if (string.IsNullOrEmpty(userId) || projectId <= 0)
            {
                return BadRequest("Invalid data.");
            }*/

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Пользователь не найден");

            // Проверка нужна, чтобы гарантировать, что запрос исключает участника только из того проекта, где он числится.
            if (!await _projectUserRepository.IsUserInProjectAsync(userId, projectId))
                return NotFound("Пользователь не состоит в проекте");
            if (!await _projectUserService.ExcludeParticipantAsync(projectId, userId))
                return StatusCode(500, "Ошибка исключения пользователя");

            return Ok(new { success = true });
            // Ваш код для удаления участника из проекта
            // ...

            //return RedirectToAction("Participants", new { projectId });
        }

        /*        [HttpPost]
                public async Task<IActionResult> ExcludeParticipant([FromBody] ExcludeParticipantDto dto)
                {
                    var user = await _userManager.FindByIdAsync(dto.UserId);
                    if (user == null)
                        return NotFound("Пользователь не найден");

                    // Проверка нужна, чтобы гарантировать, что запрос исключает участника только из того проекта, где он числится.
                    if (!await _projectUserRepository.IsUserInProjectAsync(dto.UserId, dto.ProjectId))
                        return NotFound("Пользователь не состоит в проекте");

                    if (!await _projectUserService.ExcludeParticipantAsync(dto.ProjectId, dto.UserId))
                        return StatusCode(500, "Ошибка исключения пользователя");

                    return Ok(new { success = true });
                }

                public class ExcludeParticipantDto
                {
                    public string UserId { get; set; }
                    public int ProjectId { get; set; }
                }*/
    }
}
