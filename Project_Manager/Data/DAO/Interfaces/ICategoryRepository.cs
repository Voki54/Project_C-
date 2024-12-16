using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesByProjectIdAsync(int projectId);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task DeleteByIdAsync(int categoryId);
        Task<Category> FindByIdAsync(int? categoryId);
        Task<Category?> FindByIdOrNullIncludeProjectsAsync(int categoryId);
        Task<Category> FindByIdAsNoTrackingAsync(int? categoryId);
    }
}
