using Project_Manager.Models;

namespace Project_Manager.DTO.ProjectTasks
{
    public class ProjectTaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public Category Category { get; set; }
        public string ExecutorName { get; set; }
        public DateTime DueDateTime { get; set; }
        public string? Description { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
