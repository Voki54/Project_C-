using Microsoft.EntityFrameworkCore;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;
using Project_Manager.Models.Enum;

namespace Project_Manager.Data.DAO.Repository
{
    public class ProjectUserRepository : IProjectUserRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectUser> CreateAsync(ProjectUser projectUser)
        {
            await _context.ProjectsUsers.AddAsync(projectUser);
            await _context.SaveChangesAsync();
            return projectUser;
        }

        public async Task<bool> DeleteAsync(int projectId, string userId)
        {
			var projectUser = await _context.ProjectsUsers.
                FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId);

			if (projectUser == null)
				return false;

			_context.ProjectsUsers.Remove(projectUser);
			await _context.SaveChangesAsync();
			return true;
		}

        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(string userId)
        {
            return await _context.ProjectsUsers.Where(u => u.UserId == userId)
                .Select(projectUser => new Project
                {
                    Id = projectUser.ProjectId,
                    Name = projectUser.Project.Name,
                }).ToListAsync();
        }

        public async Task<UserRoles?> GetUserRoleInProjectAsync(string userId, int projectId)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(t => t.UserId == userId && t.ProjectId == projectId);
            if (projectUser == null) return null;
            return projectUser.Role;
        }

        public Task<List<AppUser>> GetUsersByProject(Project project)
        {
            throw new NotImplementedException();
        }
    }
}
