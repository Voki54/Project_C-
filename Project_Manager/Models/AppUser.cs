﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Project_Manager.Models
{
	public class AppUser : IdentityUser
	{

        public ICollection<ProjectUser> ProjectUser { get; set; } = new List<ProjectUser>();
        public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
		public ICollection<JoinProjectRequest> JoinProjectRequest { get; set; } = new List<JoinProjectRequest>();

		//public ICollection<Notification> Notifications { get; set; }
	}
}
