using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Repository
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetCategoriesByProjectIdAsync(int projectId)
        {
            var categories = await _context.Categories.Where(t => t.ProjectId == projectId).ToListAsync();
            return categories;
        }
    }
}
