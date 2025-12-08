using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class TeamMemberController : ControllerBase
{
    private readonly ITeamMemberService _teamMemberService;

    public TeamMemberController(ITeamMemberService teamMemberService)
    {
        _teamMemberService = teamMemberService;
    }

    // 获取某团队的所有成员
    [HttpGet("{teamId}/members")]
    public async Task<IActionResult> GetMembers(Guid teamId)
    {
        var members = await _teamMemberService.GetTeamMembersAsync(teamId);
        return Ok(members);
    }
}
