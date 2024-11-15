using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Mappers;
using Project_Manager.Models;
using Project_Manager.Services;
using Project_Manager.ViewModels;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamUserRepository _teamUserRepository;
        private readonly TeamUserService _teamUserService;
        private readonly UserManager<AppUser> _userManager;

        public TeamsController(ITeamRepository teamRepository, ITeamUserRepository teamUserRepository,
            TeamUserService teamUserService, UserManager<AppUser> userManager)
        {
            _teamRepository = teamRepository;
            _teamUserRepository = teamUserRepository;
            _teamUserService = teamUserService;
            _userManager = userManager;
        }

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User is not authenticated.");

            var teams = await _teamUserService.GetUserTeamsAsync(userId);
            var teamsDTO = teams.Select(t => t.ToTeamDTO()).ToList();
            return View(teamsDTO);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAndEditTeamVM createTeamVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User is not authenticated.");

            var createdTeam = await _teamRepository.CreateAsync(createTeamVM.ToTeam());
            await _teamUserService.AddUserToTeamAsync(createdTeam.Id, userId, UserRoles.Admin);

            //return RedirectToAction("Index", "Teams");

            return RedirectToAction("Details", "Teams", new { teamId = createdTeam.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int teamId)
        {
            //TODO логика поиска задач заданной команды

            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            if (team == null) return NotFound("Team not found.");

            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User is not authenticated.");

            var userRole = await _teamUserRepository.GetUserRoleInTeamAsync(userId, teamId);
            if (userRole == null) return NotFound("User is not in the team.");

            var teamDetailsVM = new TeamDetailsVM
            {
                Team = team,
                UserRoles = (UserRoles)userRole
                //TODO список задач команды
            };

            return View(teamDetailsVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var team = await _teamRepository.GetTeamByIdAsync(id);

            if (team == null)
                return NotFound("Team not found");

            return View(team.ToCreateAndEditTeamVM());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateAndEditTeamVM editTeamVM)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("", "Failed to edit car");
                return View("Edit", editTeamVM);
            }

            await _teamRepository.UpdateAsync(
                new Team
                {
                    Id = id,
                    Name = editTeamVM.Name
                }
                );

            return RedirectToAction("Details", "Teams", new { teamId = id });
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _teamRepository.GetTeamByIdAsync(id);
            if (team == null)
            {
                return View("Error");
            }
            return View(team.ToTeamDTO());
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleteResponse = await _teamRepository.DeleteAsync(id);
            if (!deleteResponse)
            {
                return View("Error");
            }
            return RedirectToAction("Index", "Teams");
        }
    }
}
