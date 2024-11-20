using Project_Manager.DTO.ProjectTasks;
using Project_Manager.Models;
using Project_Manager.Models.Enums;

namespace Project_Manager.ViewModels
{
    public class TaskCategoryVM
    {
        public List<ProjectTaskDTO> Tasks { get; set; }        
        public List<Category> Categories { get; set; }     
        public Category SelectedCategory { get; set; }
        public string? SortedColumn { get; set; }
        public bool? IsAsc { get; set; }
        public int ProjectId { get; set; }
        public UserRoles? Role { get; set; }
    }
}
