using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.DTO.JoinProject;
using Project_Manager.Events.Notification.EventHandlers;
using Project_Manager.Events.Notification;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.ViewModels;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace Project_Manager.Controllers
{
    public class JoinProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJoinProjectRequestRepository _joinProjectRequestRepository;

        private readonly EventPublisher _eventPublisher;
        private readonly NotificationEventHandler _notificationHandler;

        public JoinProjectController(IProjectRepository projectRepository, IProjectUserRepository projectUserRepository,
            UserManager<AppUser> userManager, IJoinProjectRequestRepository joinProjectRequestRepository,
            EventPublisher eventPublisher, NotificationEventHandler notificationHandler)
        {
            _projectRepository = projectRepository;
            _projectUserRepository = projectUserRepository;
            _userManager = userManager;
            _joinProjectRequestRepository = joinProjectRequestRepository;

            _eventPublisher = eventPublisher;
            _notificationHandler = notificationHandler;

            // Подписка на событие
            _eventPublisher.Subscribe(async e => await HandleEventAsync(e));
        }

        // Обработчик события
        private async Task HandleEventAsync(IEvent @event)
        {
            if (@event is ProjectApplicationSubmittedEvent projectEvent)
            {
                await _notificationHandler.HandleAsync(projectEvent);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Join(int projectId)
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

            return View(new JoinProjectVM
            {
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

            await _joinProjectRequestRepository.CreateAsync(new JoinProjectRequest
            {
                ProjectId = projectId,
                UserId = userId,
                Status = JoinProjectRequestStatus.Pending
            });

            var @event = new ProjectApplicationSubmittedEvent(userId, projectId.ToString());
            await _eventPublisher.PublishAsync(@event);

            return RedirectToAction("Join", "JoinProject", new { projectId });
        }

        [HttpGet]
        public async Task<IActionResult> Respond(int projectId)
        {
            var usersId = await _joinProjectRequestRepository.GetUsersIdWithUnprocessedRequestsAsync(projectId);
            List<RespondDTO> respondDTOs = new List<RespondDTO>();
            AppUser? user;

            foreach (string userId in usersId)
            {
                user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    respondDTOs.Add(new RespondDTO
                    {
                        UserId = userId,
                        UserEmail = user.Email,
                        UserName = user.UserName,
                        ProjectId = projectId
                    });
                }

            }
            //удали дто joinProjectRequest
            //ViewBag.RespondDTOs = respondDTOs;
            return View(respondDTOs);
        }

        [HttpPost]
        public async Task<IActionResult> Accept(string userId, int projectId, int userRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Пользователь не найден");

            if (!await _projectUserRepository.IsUserInProjectAsync(userId, projectId))
            {
                await _projectUserRepository.CreateAsync(new ProjectUser
                {
                    UserId = userId,
                    ProjectId = projectId,
                    Role = (UserRoles)userRole
                });
            }

            await _joinProjectRequestRepository.UpdateAsync(new JoinProjectRequest
            {
                ProjectId = projectId,
                UserId = userId,
                Status = JoinProjectRequestStatus.Accepted
            });

            return RedirectToAction("Respond", new { projectId });
        }
    }
}
