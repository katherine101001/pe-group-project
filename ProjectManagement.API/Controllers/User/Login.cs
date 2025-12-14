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

        /// <summary>
        /// User Login
        /// </summary>
        [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto dto)
{
    if (dto == null)
        return BadRequest("Request body is null");

    var user = await _userService.LoginAsync(dto);

    if (user == null)
        return Unauthorized(new
        {
            message = "Invalid email, password or role"
        });

    return Ok(new
    {
        message = "Login successful",
        userId = user.Id,
        role = user.Role.ToString()
    });
}

    }
}
