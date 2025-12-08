using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITeamMemberService
{
    Task<List<TeamMemberDto>> GetTeamMembersAsync(Guid teamId);
}
