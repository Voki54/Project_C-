using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project_Manager.Models.Enums;

namespace Project_Manager.Models
{
    [Table("ProjectTask")]
    public class ProjectTask
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название задачи обязательно")]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Статус задачи обязателен")]
        public ProjectTaskStatus? Status { get; set; }

        [Required(ErrorMessage = "Выбор категории обязателен")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required(ErrorMessage = "Выбор исполнителя обязателен")]
        public string ExecutorId { get; set; }
        public AppUser? AppUser { get; set; }

        [Required(ErrorMessage = "Выбор дедлайна обязателен")]
        public DateTime DueDateTime { get; set; }

        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    }
}
