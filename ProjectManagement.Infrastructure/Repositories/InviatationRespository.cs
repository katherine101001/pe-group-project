using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities.Team;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly AppDbContext _context;

        public InvitationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Invitation> CreateAsync(Invitation invitation)
        {
            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();
            return invitation;
        }

        public async Task<Invitation?> GetByIdAsync(Guid id)
        {
            return await _context.Invitations.FindAsync(id);
        }

        public async Task<Invitation?> GetByTokenAsync(string token)
        {
            return await _context.Invitations.FirstOrDefaultAsync(i => i.Token == token);
        }

        public async Task<List<Invitation>> GetAllAsync()
        {
            return await _context.Invitations.ToListAsync();
        }

        public async Task UpdateAsync(Invitation invitation)
        {
            _context.Invitations.Update(invitation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var invitation = await GetByIdAsync(id);
            if (invitation != null)
            {
                _context.Invitations.Remove(invitation);
                await _context.SaveChangesAsync();
            }
        }
    }
}
