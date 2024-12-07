using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.Services;
using Project_Manager.ViewModels;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class ParticipantsController : Controller
    {
        //private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJoinProjectRequestRepository _joinProjectRequestRepository;
        private readonly ProjectUserService _projectUserService;

        public ParticipantsController(/*IProjectRepository projectRepository,*/ IProjectUserRepository projectUserRepository,
            UserManager<AppUser> userManager, IJoinProjectRequestRepository joinProjectRequestRepository, ProjectUserService projectUserService)
        {
            //_projectRepository = projectRepository;
            _projectUserRepository = projectUserRepository;
            _userManager = userManager;
            _joinProjectRequestRepository = joinProjectRequestRepository;
            _projectUserService = projectUserService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int projectId)
        {
            var participants = await _projectUserService.GetUserFromProjectAsync(projectId);
            var admin = participants.FirstOrDefault(p => p.Role == UserRoles.Admin);

            if (admin != null)
            {
                participants = participants.Where(p => p != admin);
            }

            return View(new ParticipantsVM
            {
                ProjectId = projectId,
                Participants = participants
                //appUsers = await _projectUserService.GetProjectUserAsync(projectId)
            }
            );
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, int projectId, UserRoles userRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Пользователь не найден");

            //нужна ли эта проуерка?
            if (!await _projectUserRepository.IsUserInProjectAsync(userId, projectId))
                return NotFound("Пользователь не состоит в проекте");

            if (!await _projectUserService.UpdateUserRoleAsync(projectId, userId, userRole))
                return NotFound("Ошибка обновления");

            return RedirectToAction("Index", new { projectId });
        }

        /*        [HttpPost]
                public async Task<IActionResult> ExcludeParticipant(string userId, int projectId)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user == null)
                        return NotFound("Пользователь не найден");

                    //нужна ли эта проуерка?
                    if (!await _projectUserRepository.IsUserInProjectAsync(userId, projectId))
                        return NotFound("Пользователь не состоит в проекте");

                    if (!await _projectUserService.ExcludeParticipantAsync(projectId, userId))
                        return NotFound("Ошибка исключения пользователя");

                    return RedirectToAction("Index", new { projectId });
                }*/

        [HttpPost]
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
        }
    }
}
