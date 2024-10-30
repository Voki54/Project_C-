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
	    public DbSet<Team> Teams { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamRoleEntity>()
                .Property(tr => tr.Role)
                .HasConversion<string>(); // Сохраняет значение enum как строку

            //// Определяем составной ключ для UserTeam
            //modelBuilder.Entity<UserTeam>()
            //    .HasKey(ut => new { ut.UserId, ut.TeamId });

            //modelBuilder.Entity<UserTeam>()
            //    .HasOne(ut => ut.User)
            //    .WithMany(u => u.UserTeams)
            //    .HasForeignKey(ut => ut.UserId);

            //modelBuilder.Entity<UserTeam>()
            //    .HasOne(ut => ut.Team)
            //    .WithMany(t => t.UserTeams)
            //    .HasForeignKey(ut => ut.TeamId);

            //// Настройка хранения enum как строки для UserRole
            //modelBuilder.Entity<UserTeam>()
            //    .Property(ut => ut.Role)
            //    .HasConversion<string>(); // Сохраняет значение enum как строку
        }

    }
}
