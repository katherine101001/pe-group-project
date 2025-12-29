using ProjectManagement.Application.DTOs.Users;
using ProjectManagement.Domain.Entities.Users;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<UserDto> RegisterAsync(RegisterDto dto);

        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<List<UserDto>> GetAllUsersAsync();
        Task UpdateUserAsync(Guid id, UserDto dto);
        Task DeleteUserAsync(Guid id);
        //Task<UserDto?> AuthenticateAsync(string email, string password);

        Task<User> InviteUserAsync(InviteTeamDto dto);
        Task<List<DisplayTeamMemberDto>> GetAllUsersSimpleAsync();

        Task<List<DisplayTeamMemberDto>> SearchUsersAsync(string keyword);

        Task<DashboardTeam> GetDashboardTeamStatsAsync();

        Task<User?> LoginAsync(LoginDto loginDto);

        Task<List<AvailableUserDto>> GetAvailableMembersAsync(Guid projectId);
        Task<List<CompleteUserDto>> GetAllCompleteUsersAsync();
    }
}