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
    [HttpPost("update-role")]
    public async Task<IActionResult> UpdateUserRole([FromBody] InviteTeamDto dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.Email))
            return BadRequest("Email is required");

        
        if (string.IsNullOrEmpty(dto.Role))
            dto.Role = "Member";

        try
        {
            await _userService.InviteUserAsync(dto);
            return Ok(new { Message = "User role updated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}