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

        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteByIdAsync(int categoryId)
        {
            Category category = await FindByIdAsync(categoryId);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> FindByIdAsync(int? categoryId)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(t => t.Id == categoryId);
            if (category == null)
            {
                throw new KeyNotFoundException($"Категория с ID {categoryId} не найдена.");
            }
            return category;
        }

        public async Task<Category> FindByIdAsNoTrackingAsync(int? categoryId)
        {
            Category category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(t => t.Id == categoryId);
            if (category == null)
            {
                throw new KeyNotFoundException($"Категория с ID {categoryId} не найдена.");
            }
            return category;
        }

        public async Task<Category?> FindByIdOrNullIncludeProjectsAsync(int categoryId)
        {
            return await _context.Categories
                        .Include(t => t.Project)
                        .FirstOrDefaultAsync(t => t.Id == categoryId);
        }
    }
}
