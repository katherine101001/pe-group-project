using ProjectManagement.Application.DTOs.Users;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto dto);

        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<List<UserDto>> GetAllUsersAsync();
        Task UpdateUserAsync(Guid id, UserDto dto);
        Task DeleteUserAsync(Guid id);
        Task<UserDto?> AuthenticateAsync(string email, string password);

        Task InviteUserAsync(InviteTeamDto dto);
    }
}