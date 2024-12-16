using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesByProjectIdAsync(int projectId);
    }
}
