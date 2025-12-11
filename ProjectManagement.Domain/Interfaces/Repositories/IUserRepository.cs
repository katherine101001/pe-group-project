using ProjectManagement.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ProjectManagement.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<Role?> GetByNameAsync(string name);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
       
        Task<List<User>> GetUsersByRolesAsync(params string[] roleNames);

        Task<List<User>> GetAllAsyncRole();



    }
}
