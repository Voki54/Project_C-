using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Project_Manager.Models
{
	public class AppUser : IdentityUser
	{
		public ICollection<ProjectUser> ProjectUser { get; set; } = new List<ProjectUser>();
		public ICollection<JoinProjectRequest> JoinProjectRequests { get; set; } = new List<JoinProjectRequest>();

		//public ICollection<Project> Projects { get; set; }
		//public ICollection<Notification> Notifications { get; set; }
	}
}
