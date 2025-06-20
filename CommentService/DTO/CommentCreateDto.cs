using System.ComponentModel.DataAnnotations;

namespace CommentService.DTOs
{
    public class CommentCreateDto
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "O texto do comentário não pode ultrapassar 1000 caracteres.")]
        public string Text { get; set; } = string.Empty;
    }
}