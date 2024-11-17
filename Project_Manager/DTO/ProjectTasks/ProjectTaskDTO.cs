using Project_Manager.Models;

namespace Project_Manager.DTO.ProjectTasks
{
    public class ProjectTaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; } // Приводим статус к строковому значению для удобства отображения
        public Category? Category { get; set; } // Название категории
        public string? ExecutorName { get; set; } // Имя исполнителя
        public DateTime DueDateTime { get; set; } // Дата завершения
        public string? Description { get; set; } // Описание задачи
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    }

}
