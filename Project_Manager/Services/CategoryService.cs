using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;
using Project_Manager.Services.Interfaces;

namespace Project_Manager.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
    }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: {@Category}", nameof(CreateCategoryAsync), category);
            return await _categoryRepository.CreateAsync(category);
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: categoryId = {CategoryId}", nameof(DeleteCategoryAsync), categoryId);
            await _categoryRepository.DeleteByIdAsync(categoryId);
        }

        public async Task<Category> FindCategoryByIdAsync(int categoryId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: categoryId = {CategoryId}", nameof(FindCategoryByIdAsync), categoryId);
            return await _categoryRepository.FindByIdAsync(categoryId);
        }

        public async Task<Category> FindCategoryByIdAsNoTrackingAsync(int categoryId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: categoryId = {CategoryId}", nameof(FindCategoryByIdAsNoTrackingAsync), categoryId);
            return await _categoryRepository.FindByIdAsNoTrackingAsync(categoryId);
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: {@Category}", nameof(UpdateCategoryAsync), category);
            return await _categoryRepository.UpdateAsync(category);
        }
    }
}
