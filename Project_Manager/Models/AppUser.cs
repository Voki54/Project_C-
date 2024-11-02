using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Project_Manager.Models
{
	public class AppUser : IdentityUser/*<string>*//*<Guid>*/
	{
		//[Key]
		//public string Id { get; set; }
		//public Guid GuidKey { get; set; } = Guid.NewGuid();
		public ICollection<TeamUser> TeamUser { get; set; } = new List<TeamUser>();
		public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
        //public ICollection<Team> Teams { get; set; }
        //public ICollection<Notification> Notifications { get; set; }
    }
}
