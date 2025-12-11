using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.Users;
using ProjectManagement.Application.Interfaces.Services;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/user/simple
    // 必须放在 id:guid 前面
    [HttpGet("Team")]
    public async Task<IActionResult> GetAllUsersSimple()
    {
        var users = await _userService.GetAllUsersSimpleAsync();
        return Ok(users);
    }

    // GET: api/user
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    // GET: api/user/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    // POST: api/user/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
    {
        var userDto = await _userService.CreateUserAsync(dto);
        return Ok(userDto);
    }
}
