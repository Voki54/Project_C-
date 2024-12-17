using System.ComponentModel.DataAnnotations;

namespace Project_Manager.ViewModels
{
    public class CreateAndEditProjectVM
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(15, ErrorMessage = "Имя проекта должно содержать не больше 15 символов")]
        public string Name { get; set; } = string.Empty;

        public CreateAndEditProjectVM() { }

        public CreateAndEditProjectVM(string name)
        {
            Name = name;
        }
        public CreateAndEditProjectVM(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }


}
