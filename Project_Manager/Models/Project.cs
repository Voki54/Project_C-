using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Manager.Models
{
    [Table("Projects")]
	public class Project
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
        public ICollection<ProjectUser> ProjectUser { get; set; } = new List<ProjectUser>();
        //public ICollection<string> TasksId { get; set; }


        //public string AdminId { get; set; }
        //public ICollection<string> ExecutorsId { get; set; }
        //public ICollection<string> ManagersId { get; set; }

    }
}
