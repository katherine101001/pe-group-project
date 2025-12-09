using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Domain.Entities.Collaborations;
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notification.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notification.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Notification notification)
        {
            _context.Notification.Remove(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetAllAsync()
        {
            return await _context.Notification.ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(Guid id)
        {
            return await _context.Notification.FindAsync(id);
        }
    }
}
