using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;

        
        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Role.FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task AddAsync(User user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.User.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync() => await _context.User.ToListAsync();

        public async Task<User?> GetByIdAsync(Guid id) => await _context.User.FindAsync(id);

        public async Task<User?> GetByEmailAsync(string email)
            {
             return await _context.User
             .Include(u => u.Role) // 包含用户的角色
             .FirstOrDefaultAsync(u => u.Email == email);
            }

//??
        public async Task<List<User>> GetUsersByRolesAsync(params string[] roleNames)
        {
         return await _context.User
        .Include(u => u.Role)
        .Where(u => roleNames.Contains(u.Role.Name))
        .ToListAsync();
        }


        public async Task<List<User>> GetAllAsyncRole()
                {
                return await _context.User
                                .Include(u => u.Role) // 确保 Role 被加载
                                .ToListAsync();
                }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.User.CountAsync();
        }

    }
}
