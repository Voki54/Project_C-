using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;

namespace Project_Manager.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            return await _categoryRepository.CreateAsync(category);
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            await _categoryRepository.DeleteByIdAsync(categoryId);
        }

        public async Task<Category> FindCategoryByIdAsync(int categoryId)
        {
            return await _categoryRepository.FindByIdAsync(categoryId);
        }

        public async Task<Category> FindCategoryByIdAsNoTrackingAsync(int categoryId)
        {
            return await _categoryRepository.FindByIdAsNoTrackingAsync(categoryId);
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            return await _categoryRepository.UpdateAsync(category);
        }
    }
}
