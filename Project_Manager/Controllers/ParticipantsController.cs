using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.DTO.AppUser;
using Project_Manager.Models.Enums;
using Project_Manager.Services;
using Project_Manager.ViewModels;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class ParticipantsController : Controller
    {
        private readonly ProjectUserService _projectUserService;

        public ParticipantsController(ProjectUserService projectUserService)
        {
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
    }
}
