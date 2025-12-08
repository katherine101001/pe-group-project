using ProjectManagement.Domain.Entities.Team;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITeamMemberRepository
{
    Task<List<TeamMember>> GetMembersByTeamIdAsync(Guid teamId);
}
