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
public async Task<IActionResult> Register([FromBody] RegisterDto dto)
{
    try
    {
        // 即使 dto.Role 是 null，也不会报错误
        var userDto = await _userService.RegisterAsync(dto);
        return Ok(new
        {
            Message = "Registration successful",
            User = userDto
        });
    }
    catch (Exception ex)
    {
        return BadRequest(new { Message = ex.Message });
    }
}



    [HttpGet("teamstats")]
    public async Task<ActionResult<DashboardTeam>> GetTeamStats()
    {
        var stats = await _userService.GetDashboardTeamStatsAsync();
        return Ok(stats);
    }

    [HttpPost("invite")]
    public async Task<IActionResult> InviteUser([FromBody] InviteTeamDto dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Role))
            return BadRequest("Email and Role are required.");

        try
        {
            var user = await _userService.InviteUserAsync(dto);
            return Ok(user); // 返回用户信息
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUserName([FromQuery] Guid userId)
    {
        if (userId == Guid.Empty)
            return BadRequest("Missing userId");

        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null) return NotFound();

        return Ok(new { Name = user.Name });
    }



}