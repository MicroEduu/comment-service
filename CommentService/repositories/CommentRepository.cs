using CommentService.Data;
using CommentService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Repositories
{
    public class CommentRepository
    {
        private readonly CommentContext _context;

        public CommentRepository(CommentContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        // Método para obter comentários pelo courseId
        public async Task<IEnumerable<Comment>> GetByCourseIdAsync(int courseId)
        {
            return await _context.Comments
                .Where(c => c.CourseId == courseId)
                .ToListAsync();
        }
    }
}
