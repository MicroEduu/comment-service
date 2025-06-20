namespace CommentService.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int CourseId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}