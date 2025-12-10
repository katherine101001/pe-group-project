using System;
using System.Threading.Tasks;
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

    //api/user/update-role
    [HttpPost("update-role")]
    public async Task<IActionResult> UpdateUserRole([FromBody] InviteTeamDto dto)
    {
        // 验证请求体是否为空或邮箱为空
        if (dto == null || string.IsNullOrEmpty(dto.Email))
            return BadRequest("Email is required");

        // 如果前端没传 Role，则默认设置为 "Member"
        if (string.IsNullOrEmpty(dto.Role))
            dto.Role = "Member";

        try
        {
            // 调用业务逻辑服务，处理邀请或更新角色
            await _userService.InviteUserAsync(dto);
            return Ok(new { Message = "User role updated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    //api/user/Team-Memberlist
    [HttpGet("Team-Memberlist")]
    public async Task<IActionResult> GetAllUsersSimple()
    {
        var users = await _userService.GetAllUsersSimpleAsync();
        return Ok(users);
    }
    //api/user/search?keyword=Alex(sample)
    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest("Keyword cannot be empty");

        var users = await _userService.SearchUersAsync(keyword);
        return Ok(users);
    }
}