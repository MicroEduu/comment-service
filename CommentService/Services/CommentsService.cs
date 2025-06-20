using CommentService.DTOs;
using CommentService.Models;
using CommentService.Repositories;

namespace CommentService.Services
{
    public class CommentsService
    {
        private readonly CommentRepository _repository;
        private readonly CourseServiceClient _courseServiceClient;

        public CommentsService(CommentRepository repository, CourseServiceClient courseServiceClient)
        {
            _repository = repository;
            _courseServiceClient = courseServiceClient;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetByCourseIdAsync(int courseId)
        {
            var courseExists = await _courseServiceClient.CourseExistsAsync(courseId);
            if (!courseExists)
                throw new KeyNotFoundException("O curso informado não existe.");

            return await _repository.GetByCourseIdAsync(courseId);
        }

        public async Task<Comment> CreateAsync(CommentCreateDto dto, int authorId)
        {
            // Verificar se o curso existe no CourseService
            var courseExists = await _courseServiceClient.CourseExistsAsync(dto.CourseId);
            if (!courseExists)
                throw new KeyNotFoundException("O curso informado não existe.");

            var comment = new Comment
            {
                AuthorId = authorId,
                CourseId = dto.CourseId,
                Text = dto.Text,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(comment);

            return comment;
        }

        public async Task<Comment> UpdatePartialAsync(int id, CommentUpdateDto dto)
        {
            var comment = await _repository.GetByIdAsync(id);
            if (comment == null)
                throw new Exception("Comentário não encontrado.");

            comment.Text = dto.Text;

            await _repository.UpdateAsync(comment);

            return comment;
        }

        public async Task<string> DeleteAsync(int id)
        {
            var comment = await _repository.GetByIdAsync(id);
            if (comment == null)
                throw new Exception("Comentário não encontrado.");

            await _repository.DeleteAsync(comment);

            return "Comentário excluído com sucesso.";
        }
    }
}