using Project_Manager.Models.Enums;

namespace Project_Manager.ViewModels
{
    public class JoinProjectVM
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public JoinProjectRequestStatus? RequestStatus { get; set; }

        public JoinProjectVM() { }

        public JoinProjectVM(int projectId, string projectName, JoinProjectRequestStatus? requestStatus)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            RequestStatus = requestStatus;
        }
    }
}
