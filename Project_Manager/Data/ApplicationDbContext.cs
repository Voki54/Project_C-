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
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
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
				.HasKey(j => new { j.ProjectId, j.UserId });

			modelBuilder.Entity<JoinProjectRequest>()
				.HasOne<Project>(p => p.Project)
				.WithMany(j => j.JoinProjectRequests)
				.HasForeignKey(jp => jp.ProjectId);

			modelBuilder.Entity<JoinProjectRequest>()
				.HasOne<AppUser>(u => u.AppUser)
				.WithMany(j => j.JoinProjectRequests)
				.HasForeignKey(ju => ju.UserId);

            modelBuilder.Entity<JoinProjectRequest>()
                .Property(j => j.Status);

			modelBuilder.Entity<Category>()
				.HasOne<Project>(ut => ut.Project)
				.WithMany(t => t.Categories)
				.HasForeignKey(ut => ut.ProjectId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<ProjectUser>()
                .Property(ut => ut.Role);

            modelBuilder.Entity<ProjectTask>()
                .HasOne(task => task.Category)     
                .WithMany(category => category.ProjectTasks) 
                .HasForeignKey(task => task.CategoryId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectTask>()
                .HasOne(task => task.AppUser)
                .WithMany(executor => executor.ProjectTasks)
                .HasForeignKey(task => task.ExecutorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectTask>()
                .HasMany(task => task.Comments) 
                .WithOne(comment => comment.ProjectTask) 
                .HasForeignKey(comment => comment.ProjectTaskId) 
                .OnDelete(DeleteBehavior.Cascade);

        }

	}
}
