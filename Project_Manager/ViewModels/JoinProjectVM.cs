using Project_Manager.Models.Enum;

namespace Project_Manager.ViewModels
{
    public class JoinProjectVM
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public JoinProjectRequestStatus? RequestStatus { get; set; }
    }
}
