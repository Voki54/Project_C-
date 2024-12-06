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
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<JoinProjectRequest> JoinProjectRequests { get; set; } = new List<JoinProjectRequest>();
	}
}
