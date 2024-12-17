using Project_Manager.DTO.AppUser;

namespace Project_Manager.ViewModels
{
    public class ParticipantsVM
    {
        public int ProjectId { get; set; }
        public IEnumerable<AppUserDTO> Participants { get; set; }

        public ParticipantsVM(int projectId, IEnumerable<AppUserDTO> participants) 
        { 
            ProjectId = projectId;
            Participants = participants;
        }
    }
}
