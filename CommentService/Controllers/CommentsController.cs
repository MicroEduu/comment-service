using CommentService.DTOs;
using CommentService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CommentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentsService _service;

        public CommentController(CommentsService service)
        {
            _service = service;
        }

        // GET ALL - Admin, Teacher, Student
        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _service.GetAllAsync();
            if (comments == null || !comments.Any())
                return NotFound("Nenhum coment�rio encontrado");

            return Ok(comments);
        }

        // GET BY ID - Admin, Teacher, Student
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _service.GetByIdAsync(id);
            if (comment == null)
                return NotFound("Coment�rio n�o encontrado");

            return Ok(comment);
        }

        // Buscar coment�rios por courseId
        [HttpGet("by-course/{courseId}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> GetCommentsByCourse(int courseId)
        {
            var comments = await _service.GetByCourseIdAsync(courseId);

            if (comments == null || !comments.Any())
                return NotFound("Nenhum coment�rio encontrado para este curso.");

            return Ok(comments);
        }

        // CREATE - Admin, Teacher, Student
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("Token inv�lido: Id do usu�rio n�o encontrado");

                var authorId = int.Parse(userIdClaim.Value);

                var createdComment = await _service.CreateAsync(dto, authorId);

                return CreatedAtAction(nameof(GetCommentById), new { id = createdComment.Id }, createdComment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar coment�rio: {ex.Message}");
            }
        }

        // UPDATE - Admin ou pr�prio autor
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> UpdateCommentPartial(int id, [FromBody] CommentUpdateDto dto)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("Token inv�lido: Id do usu�rio n�o encontrado");

                var userId = int.Parse(userIdClaim.Value);

                var comment = await _service.GetByIdAsync(id);
                if (comment == null)
                    return NotFound("Coment�rio n�o encontrado");

                // Se n�o for admin, verifica se � o autor do coment�rio
                if (!User.IsInRole("Admin") && comment.AuthorId != userId)
                    return Forbid("Apenas o autor ou um admin pode editar este coment�rio.");

                var updatedComment = await _service.UpdatePartialAsync(id, dto);
                return Ok(updatedComment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar coment�rio: {ex.Message}");
            }
        }

        // DELETE - Admin ou pr�prio autor
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("Token inv�lido: Id do usu�rio n�o encontrado");

                var userId = int.Parse(userIdClaim.Value);

                var comment = await _service.GetByIdAsync(id);
                if (comment == null)
                    return NotFound("Coment�rio n�o encontrado");

                // Se n�o for admin, verifica se � o autor
                if (!User.IsInRole("Admin") && comment.AuthorId != userId)
                    return Forbid("Apenas o autor ou um admin pode excluir este coment�rio.");

                var message = await _service.DeleteAsync(id);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir coment�rio: {ex.Message}");
            }
        }
    }
}