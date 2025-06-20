using System.ComponentModel.DataAnnotations;

namespace CommentService.DTOs
{
	public class CommentUpdateDto
	{
		[Required]
		[StringLength(1000, ErrorMessage = "O texto do coment�rio n�o pode ultrapassar 1000 caracteres.")]
		public string Text { get; set; } = string.Empty;
	}
}