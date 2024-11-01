using Project_Manager.DTO.Team;
using Project_Manager.Models;

namespace Project_Manager.Mappers
{
    public static class TeamMapper
    {
        public static TeamDTO ToStockDto(this Team team)
        {
            return new TeamDTO
            {
                Id = team.Id,
                Name = team.Name
            };
        }

        public static Team ToTeamFromCreateDTO(this CreateTeamRequestDTO teamDTO)
        {
            return new Team
            {
                Name = teamDTO.Name
            };
        }
    }
}
