using Project_Manager.Models;
using Project_Manager.Models.Enums;

namespace Project_Manager.ViewModels
{
    public class ProjectDetailsVM
    {
        public Project Project { get; set; }
        public UserRoles UserRoles { get; set; }
        //TODO список задач команды
    }
}
