using Microsoft.AspNetCore.Mvc;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Data.DAO.Repository;
using Project_Manager.Models;
using Project_Manager.Models.Enum;
using Project_Manager.ViewModels;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    public class JoinProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IJoinProjectRequestRepository _joinProjectRequestRepository;

        public JoinProjectController(IProjectRepository projectRepository, IJoinProjectRequestRepository joinProjectRequestRepository)
        {
            _projectRepository = projectRepository;
            _joinProjectRequestRepository = joinProjectRequestRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int projectId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
                return NotFound("Команда не найдена.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            JoinProjectRequestStatus? requestStatus;
            var joinProjectRequest = await _joinProjectRequestRepository.GetJoinProjectRequestAsync(projectId, userId);
            if (joinProjectRequest == null)
                requestStatus = null;
            else
                requestStatus = joinProjectRequest.Status;

            return View(new JoinProjectVM { 
                ProjectId = project.Id, 
                ProjectName = project.Name,
                RequestStatus = requestStatus
			});
        }

		[HttpPost]
		public async Task<IActionResult> SubmitJoinRequest(int projectId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized();

            //var existingRequest = await _teamUserRequestRepository.GetRequestByTeamAndUserAsync(teamId, userId);
            //if (existingRequest != null)
            //    return BadRequest("Вы уже подали заявку на вступление.");

            await _joinProjectRequestRepository.CreateAsync(new JoinProjectRequest
            {
                ProjectId = projectId,
                UserId = userId,
                Status = JoinProjectRequestStatus.Pending
            });

            return RedirectToAction("Index", "JoinProject", new { projectId });
		}
	}
}
