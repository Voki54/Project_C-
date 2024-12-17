using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Repository
{
    public class CommentRepository: ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }
    }
}
