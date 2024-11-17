using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Models;

namespace Project_Manager.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Project> Projects { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<ProjectUser> ProjectsUsers { get; set; }
		public DbSet<JoinProjectRequest> JoinProjectRequests { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			modelBuilder.Entity<ProjectUser>()
				.HasKey(pu => new { pu.ProjectId, pu.UserId });
			
			modelBuilder.Entity<ProjectUser>()
				.HasOne<Project>(pu => pu.Project)
				.WithMany(p => p.ProjectUser)
				.HasForeignKey(pu => pu.ProjectId);

			modelBuilder.Entity<ProjectUser>()
				.HasOne<AppUser>(pu => pu.AppUser)
				.WithMany(u => u.ProjectUser)
				.HasForeignKey(ut => ut.UserId);

			modelBuilder.Entity<ProjectUser>()
				.Property(pu => pu.Role);


			modelBuilder.Entity<JoinProjectRequest>()
				.HasKey(pu => new { pu.ProjectId, pu.UserId });

			modelBuilder.Entity<JoinProjectRequest>()
				.HasOne<Project>(pu => pu.Project)
				.WithMany(p => p.JoinProjectRequests)
				.HasForeignKey(pu => pu.ProjectId);


			modelBuilder.Entity<JoinProjectRequest>()
				.HasOne<AppUser>(pu => pu.AppUser)
				.WithMany(u => u.JoinProjectRequests)
				.HasForeignKey(ut => ut.UserId);

			modelBuilder.Entity<JoinProjectRequest>()
				.Property(pu => pu.Status);
		}

	}
}
