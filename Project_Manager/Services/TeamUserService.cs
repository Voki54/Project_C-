using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;

namespace Project_Manager.Services
{
    public class TeamUserService
    {
        private readonly ITeamUserRepository _teamUserRepository;

        public TeamUserService(ITeamUserRepository teamUserRepository)
        {
            _teamUserRepository = teamUserRepository;
        }

        public async Task<IEnumerable<Team>> GetUserTeamsAsync(string userId)
        {
            return await _teamUserRepository.GetTeamsByUserIdAsync(userId);
        }

        public async Task<bool> AddUserToTeamAsync(int teamId, string userId, UserRoles userRole)
        {
            await _teamUserRepository.CreateAsync(
                new TeamUser
                {
                    TeamId = teamId,
                    UserId = userId,
                    Role = userRole
                }
                );
            return true;
        }
    }

}
