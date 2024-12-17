using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Manager.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название категории обязательно")]
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    }
}
