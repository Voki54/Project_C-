using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Manager.Models
{
    [Table("Teams")]
	public class Team
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
        public ICollection<TeamUser> TeamUser { get; set; } = new List<TeamUser>();
        //public ICollection<string> TasksId { get; set; }

        //public string AdminId { get; set; }
        //public ICollection<string> ExecutorsId { get; set; }
        //public ICollection<string> ManagersId { get; set; }

    }
}
