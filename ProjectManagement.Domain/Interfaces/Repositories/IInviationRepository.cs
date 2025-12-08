using ProjectManagement.Domain.Entities.Team;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Interfaces.Repositories
{
    public interface IInvitationRepository
    {
        Task<Invitation> CreateAsync(Invitation invitation);
        Task<Invitation?> GetByIdAsync(Guid id);
        Task<Invitation?> GetByTokenAsync(string token);
        Task<List<Invitation>> GetAllAsync();
        Task UpdateAsync(Invitation invitation);
        Task DeleteAsync(Guid id);
    }
}
