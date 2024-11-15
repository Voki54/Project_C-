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
	    public DbSet<Team> Teams { get; set; }
		public DbSet<Notification> Notifications { get; set; }
        public DbSet<TeamUser> TeamsUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*modelBuilder.Entity<AppUser>()
	            .HasAlternateKey(u => u.GuidKey);*/

            modelBuilder.Entity<TeamUser>()
                .HasKey(ut => new { ut.UserId, ut.TeamId });

            modelBuilder.Entity<TeamUser>()
                .HasOne<AppUser>(ut => ut.AppUser)
                .WithMany(u => u.TeamUser)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<TeamUser>()
                .HasOne<Team>(ut => ut.Team)
                .WithMany(t => t.TeamUser)
                .HasForeignKey(ut => ut.TeamId);

            modelBuilder.Entity<TeamUser>()
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
