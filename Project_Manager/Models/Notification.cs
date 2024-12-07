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
		public DateTime SendDate { get; set; }
		//TODO решить с типом события!
		//public string eventNotification {  get; set; }

	}
}
