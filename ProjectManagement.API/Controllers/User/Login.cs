using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.Users;
using ProjectManagement.Application.Interfaces.Services;

namespace ProjectManagement.API.Controllers.User
{
    [ApiController]
    [Route("api/user")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Request body is null" });

            if (string.IsNullOrWhiteSpace(dto.Role))
                return BadRequest(new { message = "Role is required" });

            // ✅ 统一 role 格式
            dto.Role = dto.Role.Trim().ToUpper();

            var allowedRoles = new[] { "ADMIN", "MANAGER", "MEMBER" };
            if (!allowedRoles.Contains(dto.Role))
            {
                return BadRequest(new
                {
                    message = "Invalid role. Allowed roles: ADMIN, MANAGER, MEMBER"
                });
            }

            var user = await _userService.LoginAsync(dto);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email, password, or role"
                });
            }

            return Ok(new
            {
                message = "Login successful",
                userId = user.Id,
                userName = user.Name ?? "User",
                role = user.Role?.Name?.ToUpper() ?? "MEMBER"
            });
        }
    }
}
