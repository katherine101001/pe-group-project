using ProjectManagement.Domain.Entities.Team;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface IInvitationService
    {
        Task<Invitation> InviteUserAsync(string email, string role);
        Task<Invitation?> GetInvitationByIdAsync(Guid id);
        Task<Invitation?> GetInvitationByTokenAsync(string token);
        Task<List<Invitation>> GetAllInvitationsAsync();
        Task UpdateInvitationAsync(Invitation invitation);
        Task DeleteInvitationAsync(Guid id);
    }
}
