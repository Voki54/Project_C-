using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Models;

namespace Project_Manager.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser/*, IdentityRole<string>, string*//*, IdentityRole<Guid>, Guid*/>
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*modelBuilder.Entity<AppUser>()
	            .HasAlternateKey(u => u.GuidKey);*/

            modelBuilder.Entity<ProjectUser>()
                .HasKey(ut => new { ut.UserId, ut.ProjectId });

            modelBuilder.Entity<ProjectUser>()
                .HasOne<AppUser>(ut => ut.AppUser)
                .WithMany(u => u.ProjectUser)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<ProjectUser>()
                .HasOne<Project>(ut => ut.Project)
                .WithMany(t => t.ProjectUser)
                .HasForeignKey(ut => ut.ProjectId);

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
