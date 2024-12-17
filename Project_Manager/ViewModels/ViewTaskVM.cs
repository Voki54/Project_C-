using Project_Manager.DTO.ProjectTasks;
using Project_Manager.Models.Enums;

namespace Project_Manager.ViewModels
{
    public class ViewTaskVM
    {
        public ProjectTaskDTO Task { get; set; }
        public int ProjectId { get; set; }
        public UserRoles Role { get; set; }
    }
}
