using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Manager.Models
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now; 

        public int ProjectTaskId { get; set; } 
        public ProjectTask ProjectTask { get; set; } 
    }
}
