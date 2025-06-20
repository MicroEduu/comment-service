using CommentService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("token")]
    public async Task<IActionResult> GetToken([FromQuery] string email, [FromQuery] string password)
    {
        try
        {
            var token = await _authService.GetTokenAsync(email, password);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("user-info")]
    [Authorize]
    public IActionResult TestAuth()
    {
        var userId = User.FindFirst("nameid")?.Value;
        var email = User.FindFirst("email")?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var roleCustom = User.FindFirst("role")?.Value;

        return Ok(new
        {
            UserId = userId,
            Email = email,
            RoleFromClaimTypes = role,
            RoleFromCustomClaim = roleCustom,
            AllClaims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}
