using System.ComponentModel.DataAnnotations;

namespace Project_Manager.Models
{
	public class Notification
	{
		[Key]
		public string Id { get; set; }
		public string Message { get; set; }
		public ICollection<string> recipientsId { get; set; }
		public DateOnly sendDate { get; set; }
		//TODO решить с типом события!
		public string eventNotification {  get; set; }

	}
}
