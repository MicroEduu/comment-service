using Microsoft.EntityFrameworkCore;
using CommentService.Models;

namespace CommentService.Data
{
    public class CommentContext : DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options) : base(options) { }

        public DbSet<Comment> Comments => Set<Comment>();
    }
}