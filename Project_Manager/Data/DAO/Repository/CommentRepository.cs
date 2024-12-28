using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Models;

namespace Project_Manager.Data.DAO.Repository
{
    public class CommentRepository: ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CommentRepository> _logger;

        public CommentRepository(ApplicationDbContext context, ILogger<CommentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateAsync(Comment comment)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: {@Comment}", nameof(CreateAsync), comment);
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Комментарий с ID {CommentId} успешно создан.", comment.Id);
        }
    }
}
