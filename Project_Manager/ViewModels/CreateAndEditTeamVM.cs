using System.ComponentModel.DataAnnotations;

namespace Project_Manager.ViewModels
{
    public class CreateAndEditTeamVM
    {
        [Required]
        [MaxLength(15, ErrorMessage = "Team name cannot be over 15 over characters")]
        public string Name { get; set; } = string.Empty;
    }
}
