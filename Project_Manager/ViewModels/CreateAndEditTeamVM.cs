using System.ComponentModel.DataAnnotations;

namespace Project_Manager.ViewModels
{
    public class CreateAndEditProjectVM
    {
        [Required]
        [MaxLength(15, ErrorMessage = "Project name cannot be over 15 over characters")]
        public string Name { get; set; } = string.Empty;
    }
}
