using Project_Manager.Models;

namespace Project_Manager.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int categoryId);
        Task<Category> FindCategoryByIdAsync(int categoryId);
        Task<Category> FindCategoryByIdAsNoTrackingAsync(int categoryId);
        Task<Category> UpdateCategoryAsync(Category category);
    }
}
