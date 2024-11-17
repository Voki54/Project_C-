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
	    public DbSet<Project> Projects { get; set; }
		public DbSet<Notification> Notifications { get; set; }
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

            modelBuilder.Entity<ProjectUser>()
                .Property(ut => ut.Role);
        }

    }
}
