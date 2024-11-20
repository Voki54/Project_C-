using Project_Manager.Models.Enums;

namespace Project_Manager.ViewModels
{
    public class ProjectDetailsVM
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public UserRoles UserRoles { get; set; }
        public string InvitationLink { get; set; }
        //TODO список задач команды
    }
}
