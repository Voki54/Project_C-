using Microsoft.AspNetCore.Identity;

namespace Project_Manager.Models
{
	public class AppUser : IdentityUser
	{

        public ICollection<ProjectUser> ProjectUser { get; set; } = new List<ProjectUser>();
        public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
		public ICollection<JoinProjectRequest> JoinProjectRequests { get; set; } = new List<JoinProjectRequest>();

		//public ICollection<Notification> Notifications { get; set; }
	}
}
