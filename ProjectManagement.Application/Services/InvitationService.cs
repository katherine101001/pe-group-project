using ProjectManagement.Domain.Entities.Team;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Services
{
    public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository _invitationRepo;
    private readonly IMapper _mapper;

    public InvitationService(IInvitationRepository invitationRepository, IMapper mapper)
    {
        _invitationRepo = invitationRepository;
        _mapper = mapper;
    }

    public async Task<Invitation> InviteUserAsync(string email, string role)
    {
        var invitation = new Invitation
        {
            InvitedEmail = email,
            TargetRole = role,
            Token = Guid.NewGuid().ToString("N"),
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        return await _invitationRepo.CreateAsync(invitation);
    }

    public async Task<Invitation?> GetInvitationByIdAsync(Guid id)
        => await _invitationRepo.GetByIdAsync(id);

    public async Task<Invitation?> GetInvitationByTokenAsync(string token)
        => await _invitationRepo.GetByTokenAsync(token);

    public async Task<List<Invitation>> GetAllInvitationsAsync()
        => await _invitationRepo.GetAllAsync();

    public async Task UpdateInvitationAsync(Invitation invitation)
        => await _invitationRepo.UpdateAsync(invitation);

    public async Task DeleteInvitationAsync(Guid id)
        => await _invitationRepo.DeleteAsync(id);
}

}
