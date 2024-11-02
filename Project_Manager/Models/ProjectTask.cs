using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project_Manager.Models.Enums;

namespace Project_Manager.Models
{
    [Table("Task")]
    public class ProjectTask
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ExecutorId { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime DueDateTime { get; set; }
    }
}
