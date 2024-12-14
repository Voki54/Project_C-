using Project_Manager.DTO.Users;
using Project_Manager.Models;

namespace Project_Manager.ViewModels
{
    public class CreateReadTaskVM
    {
        public ProjectTask? Task { get; set; }
        public int ProjectId { get; set; }
        public List<Category> Categories { get; set; }
        public List<UserDTO> Users { get; set; }
    }
}
