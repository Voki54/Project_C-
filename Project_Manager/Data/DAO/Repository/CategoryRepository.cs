using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;
using Project_Manager.Services;

namespace Project_Manager.Data.DAO.Repository
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Category>> GetCategoriesByProjectIdAsync(int projectId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: projectId = {ProjectId}", nameof(GetCategoriesByProjectIdAsync), projectId);
            var categories = await _context.Categories.Where(t => t.ProjectId == projectId).ToListAsync();
            _logger.LogInformation("Количество категорий, найденных для проекта с ID {ProjectId}: {Count}", projectId, categories.Count);
            return categories;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: {@Category}", nameof(CreateAsync), category);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Категория с ID {CategoryId} успешно создана.", category.Id);
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: {@Category}", nameof(UpdateAsync), category);
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Категория с ID {CategoryId} успешно обновлена.", category.Id);
            return category;
        }

        public async Task DeleteByIdAsync(int categoryId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: categoryId = {CategoryId}", nameof(DeleteByIdAsync), categoryId);
            Category category = await FindByIdAsync(categoryId);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Категория с ID {CategoryId} успешно удалена.", categoryId);
        }

        public async Task<Category> FindByIdAsync(int? categoryId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: categoryId = {CategoryId}", nameof(FindByIdAsync), categoryId);
            Category category = await _context.Categories.FirstOrDefaultAsync(t => t.Id == categoryId);
            if (category == null)
            {
                _logger.LogError("Категория с ID {CategoryId} не найдена.", categoryId);
                throw new KeyNotFoundException($"Категория с ID {categoryId} не найдена.");
            }
            _logger.LogInformation("Категория с ID {CategoryId} найдена.", categoryId);
            return category;
        }

        public async Task<Category> FindByIdAsNoTrackingAsync(int? categoryId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: categoryId = {CategoryId}", nameof(FindByIdAsNoTrackingAsync), categoryId);
            Category category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(t => t.Id == categoryId);
            if (category == null)
            {
                _logger.LogError("Категория с ID {CategoryId} не найдена.", categoryId);
                throw new KeyNotFoundException($"Категория с ID {categoryId} не найдена.");
            }
            _logger.LogInformation("Категория без отслеживания с ID {CategoryId} найдена.", categoryId);
            return category;
        }

        public async Task<Category?> FindByIdOrNullIncludeProjectsAsync(int categoryId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: categoryId = {CategoryId}", nameof(FindByIdOrNullIncludeProjectsAsync), categoryId);
            return await _context.Categories
                        .Include(t => t.Project)
                        .FirstOrDefaultAsync(t => t.Id == categoryId);
        }
    }
}
