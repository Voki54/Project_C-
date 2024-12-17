using Microsoft.EntityFrameworkCore;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Repository
{
	public class ProjectRepository : IProjectRepository
	{
		private readonly ApplicationDbContext _context;

		public ProjectRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Project?> GetProjectByIdAsync(int id)
		{
			return await _context.Projects.FindAsync(id);
		}

        public async Task<string?> GetProjectNameAsync(int id)
        {
			var project = await _context.Projects.FindAsync(id);
			if (project == null)
				return null;

			return project.Name;
        }

        public async Task<Project> CreateAsync(Project project)
		{
			await _context.Projects.AddAsync(project);
			await _context.SaveChangesAsync();
			return project;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var project = await _context.Projects.FindAsync(id);

			if (project == null)
				return false;

			_context.Projects.Remove(project);
			await _context.SaveChangesAsync();
			return true;
		}

        public async Task<bool> ExistProjectAsync(int projectId)
        {
            return await _context.Projects.AnyAsync(p => p.Id == projectId);
        }

        public async Task<bool> UpdateAsync(Project project)
		{
			var existingProject = await _context.Projects.FindAsync(project.Id);

			if (existingProject == null)
				return false;

			existingProject.Name = project.Name;
			await _context.SaveChangesAsync();
			return true;
		}
	}
}

