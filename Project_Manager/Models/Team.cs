using System.ComponentModel.DataAnnotations;

namespace Project_Manager.Models
{
	public class Team
	{
		[Key]
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string AdminId { get; set; }
		public ICollection<string> ExecutorsId { get; set; }
		public ICollection<string> ManagersId { get; set; }

	}
}
