using AutoMapper;
using ProjectManagement.Application.DTOs.Users;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Application.Interfaces.Services;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Shared.Exceptions;

namespace ProjectManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        // {
        //     // Map input DTO -> Entity
        //     var user = _mapper.Map<User>(dto);

        //     await _userRepository.AddAsync(user);

        //     // Map Entity -> DTO to return
        //     return _mapper.Map<UserDto>(user);
        // }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
{
    var user = _mapper.Map<User>(dto);

    // 自动设置 RoleId 为 Admin
    var adminRole = await _userRepository.GetByNameAsync("Admin");
    if (adminRole == null)
        throw new Exception("Admin role not found in database");

    user.RoleId = adminRole.Id;

    await _userRepository.AddAsync(user);

    return _mapper.Map<UserDto>(user);
}




        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task UpdateUserAsync(Guid id, UserDto dto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);

            if (existingUser == null)
                throw new NotFoundException("User not found");

            // Map updated DTO values onto entity
            _mapper.Map(dto, existingUser);

            await _userRepository.UpdateAsync(existingUser);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);

            if (existingUser == null)
                throw new NotFoundException("User not found");

            await _userRepository.DeleteAsync(existingUser);
        }

        // public async Task<UserDto?> AuthenticateAsync(string email, string password)
        // {
        //     var user = await _userRepository.GetByEmailAsync(email);

        //     if (user == null)
        //         return null;

        //     if (user.Password != password)
        //         return null;

        //     return _mapper.Map<UserDto>(user);
        // }




public async Task<User> InviteUserAsync(InviteTeamDto dto)
        {
            // 1. 检查邮箱是否已存在
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception($"User with email '{dto.Email}' already exists.");

            // 2. 根据角色名称查找角色
            var role = await _userRepository.GetByNameAsync(dto.Role);
            if (role == null)
                throw new Exception($"Role '{dto.Role}' not found.");

            // 3. 创建 User 实体并赋值
            var user = _mapper.Map<User>(dto);
            user.RoleId = role.Id;           // 给用户赋角色
            user.Name = "New User";          // 默认名字
            user.Password = "12345";         // 默认密码

            // 4. 保存用户
            await _userRepository.AddAsync(user);

            return user;
        }

public async Task<List<DisplayTeamMemberDto>> GetAllUsersSimpleAsync()
{
    var users = await _userRepository.GetAllAsyncRole(); // 从数据库拿所有用户
    return users.Select(u => new DisplayTeamMemberDto
    {
        Name = u.Name ?? "Unknown",
        Email = u.Email,
        Role = u.Role != null ? u.Role.Name : "Unknown" // 防止 Role 为 null
    }).ToList();
}


        public async Task<List<DisplayTeamMemberDto>> SearchUsersAsync(string keyword)
{
    if (string.IsNullOrEmpty(keyword))
        return new List<DisplayTeamMemberDto>();

    var users = await _userRepository.GetAllAsyncRole();

    var filtered = users.Where(u =>
        (!string.IsNullOrEmpty(u.Name) && u.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
        (!string.IsNullOrEmpty(u.Email) && u.Email.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
        (u.Role != null && u.Role.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
    ).ToList();

    return filtered.Select(u => new DisplayTeamMemberDto
    {
        Name = u.Name ?? "Unknown",
        Email = u.Email,
        Role = u.Role != null ? u.Role.Name : "Unknown"
    }).ToList();
}





        }

    }

