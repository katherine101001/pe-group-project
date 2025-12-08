using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TeamMemberRepository : ITeamMemberRepository
{
    private readonly AppDbContext _context;

    public TeamMemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TeamMember>> GetMembersByTeamIdAsync(Guid teamId)
    {
        return await _context.TeamMembers
            .Include(tm => tm.User) // 关联 User 实体
            .Where(tm => tm.TeamId == teamId)
            .ToListAsync();
    }
}
