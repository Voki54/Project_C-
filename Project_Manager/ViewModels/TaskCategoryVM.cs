using Project_Manager.Models;

namespace Project_Manager.ViewModels
{
    public class TaskCategoryVM
    {
        public List<ProjectTask> Tasks { get; set; }        
        public List<Category> Categories { get; set; }     
        public Category SelectedCategory { get; set; }       
    }
}
