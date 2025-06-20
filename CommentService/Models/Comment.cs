using System.ComponentModel.DataAnnotations;

namespace CommentService.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int CourseId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}