using Project_Manager.Models.Enums;

namespace Project_Manager.DTO.JoinProject
{
    public class JoinProjectRequestDTO
    {
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public JoinProjectRequestStatus Status { get; set; }
    }
}
