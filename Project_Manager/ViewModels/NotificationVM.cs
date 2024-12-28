using Project_Manager.Models.Enums;

namespace Project_Manager.ViewModels
{
    public class NotificationVM
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public NotificationState State { get; set; }
        public DateTime SendDate { get; set; }
    }
}
