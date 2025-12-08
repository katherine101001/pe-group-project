using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TeamMemberService : ITeamMemberService
{
    private readonly ITeamMemberRepository _repository;
    private readonly IMapper _mapper;

    public TeamMemberService(ITeamMemberRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<TeamMemberDto>> GetTeamMembersAsync(Guid teamId)
    {
        var members = await _repository.GetMembersByTeamIdAsync(teamId);
        return _mapper.Map<List<TeamMemberDto>>(members);
    }
}
