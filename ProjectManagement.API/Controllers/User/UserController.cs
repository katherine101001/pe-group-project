using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.Users;
using ProjectManagement.Application.Interfaces.Services;

namespace ProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService; 

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            try
            {
                var userDto = await _userService.CreateUserAsync(dto);
                return Ok(userDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // // POST: api/user/login
        // [HttpPost("login")]
        // public async Task<IActionResult> Login([FromBody] LoginDto dto)
        // {
        //     var userDto = await _userService.AuthenticateAsync(dto.Email, dto.Password);

        //     if (userDto == null)
        //         return Unauthorized(new { message = "Invalid credentials" });

        //     return Ok(userDto);
        // }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        
    }
}
