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

		//TODO добавить возможность обработки разных sql запросов
		public async Task<IEnumerable<Project>> GetAllAsync()
		{
			return await _context.Projects.ToListAsync();
		}

		public async Task<Project?> GetProjectByIdAsync(int id)
		{
			return await _context.Projects.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<Project> CreateAsync(Project project)
		{
			await _context.Projects.AddAsync(project);
			await _context.SaveChangesAsync();
			return project;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);

			if (project == null)
				return false;

			_context.Projects.Remove(project);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> UpdateAsync(Project project)
		{
			var existingProject = await _context.Projects.FindAsync(project.Id);

			if (existingProject == null)
			{
				return false;
			}

			existingProject.Name = project.Name;
			await _context.SaveChangesAsync();
			return true;
		}
	}
}

