using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface ICommentRepository
    {
        Task CreateAsync(Comment comment);
    }
}
