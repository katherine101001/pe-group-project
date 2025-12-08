using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Interfaces.Services;

[ApiController]
[Route("api/[controller]")]
public class InvitationController : ControllerBase
{
    private readonly IInvitationService _invitationService;

    public InvitationController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    // 1️⃣ 邀请用户
    [HttpPost("invite")]
    public async Task<IActionResult> Invite([FromBody] InviteMemberDto dto)
    {
        try
        {
            var invitation = await _invitationService.InviteUserAsync(dto.Email, dto.Role);
            return Ok(invitation); // 如果用 DTO 映射可以改成 Response DTO
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 2️⃣ 获取所有邀请
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var invitations = await _invitationService.GetAllInvitationsAsync();
        return Ok(invitations);
    }

    // 3️⃣ 根据 Id 获取邀请
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var invitation = await _invitationService.GetInvitationByIdAsync(id);
        if (invitation == null)
            return NotFound();
        return Ok(invitation);
    }

    // 4️⃣ 根据 Token 获取邀请
    [HttpGet("token/{token}")]
    public async Task<IActionResult> GetByToken(string token)
    {
        var invitation = await _invitationService.GetInvitationByTokenAsync(token);
        if (invitation == null)
            return NotFound();
        return Ok(invitation);
    }

    // 5️⃣ 删除邀请
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _invitationService.DeleteInvitationAsync(id);
        return NoContent();
    }
}
