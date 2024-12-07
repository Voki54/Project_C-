using Microsoft.EntityFrameworkCore;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.DTO.AppUser;
using Project_Manager.Models;
using Project_Manager.Models.Enums;

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
                FindAsync(projectId, userId);

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
                    Name = projectUser.Project.Name
                }).ToListAsync();
        }

        public async Task<UserRoles?> GetUserRoleInProjectAsync(string userId, int projectId)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(t => t.UserId == userId && t.ProjectId == projectId);
            if (projectUser == null) return null;
            return projectUser.Role;
        }

        public async Task<string?> GetAdminIdInProjectAsync(int projectId)
        {
            var projectUser = await _context.ProjectsUsers.FirstOrDefaultAsync(p => p.ProjectId == projectId && p.Role == UserRoles.Admin);
            if (projectUser == null) return null;
            return projectUser.UserId;
        }

        public async Task<IEnumerable<AppUserDTO>> GetUsersByProjectIdAsync(int projectId)
        {
            return await _context.ProjectsUsers.Where(p => p.ProjectId == projectId)
                .Select(projectUser => new AppUserDTO
                {
                    Id = projectUser.UserId,
                    Name = projectUser.AppUser.UserName,
                    Email = projectUser.AppUser.Email,
                    Role = projectUser.Role
                }).ToListAsync();
        }

        public async Task<bool> IsUserInProjectAsync(string userId, int projectId)
        {
            if (await _context.ProjectsUsers.FirstOrDefaultAsync(p => p.UserId == userId && p.ProjectId == projectId) == null)
                return false;
            return true;
        }

        public async Task<bool> UpdateAsync(ProjectUser projectUser)
        {
            var existingProjectUser = await _context.ProjectsUsers.FindAsync(projectUser.ProjectId, projectUser.UserId);

            if (existingProjectUser == null)
            {
                return false;
            }

            existingProjectUser.Role = projectUser.Role;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
