using Project_Manager.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Manager.Models
{
	[Table("Notifications")]
	public class Notification
	{
		[Key]
		public int Id { get; set; }
		public string Message { get; set; }
		public string RecipientId { get; set; }
        public NotificationState State { get; set; } = NotificationState.Created;
        public DateTime SendDate { get; set; } = DateTime.UtcNow;
        public AppUser AppUser { get; set; }
    }
}
