using Project_Manager.Models.Enums;

namespace Project_Manager.DTO.Notification
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public NotificationState State { get; set; }
        public DateTime SendDate { get; set; }
    }
}
