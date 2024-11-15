using Project_Manager.DTO.Team;
using Project_Manager.Models;
using Project_Manager.ViewModels;

namespace Project_Manager.Mappers
{
    public static class TeamMapper
    {
        public static TeamDTO ToTeamDTO(this Team team)
        {
            return new TeamDTO
            {
                Id = team.Id,
                Name = team.Name
            };
        }

        public static Team ToTeam(this CreateAndEditTeamVM createTeamVM)
        {
            return new Team
            {
                Name = createTeamVM.Name
            };
        }

        public static EditTeamDTO ToEditTeamDTO(this Team team)
        {
            return new EditTeamDTO
            {
                Name = team.Name
            };
        }

        public static Team ToTeam(this EditTeamDTO createAndEditTeamDTO)
        {
            return new Team
            {
                Name = createAndEditTeamDTO.Name
            };
        }

        public static CreateAndEditTeamVM ToCreateAndEditTeamVM(this Team team)
        {
            return new CreateAndEditTeamVM
            {
                Name = team.Name
            };
        }
    }
}
