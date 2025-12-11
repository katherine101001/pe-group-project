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


//未完成
        public async Task InviteUserAsync(InviteTeamDto dto)
{
    var user = await _userRepository.GetByEmailAsync(dto.Email);
    if (user == null)
    {
        throw new Exception($"User with email '{dto.Email}' not found");
    }

    // 默认角色为 Member
    string roleName = string.IsNullOrEmpty(dto.Role) ? "Member" : dto.Role;

    // 直接对应数据库中已有的 RoleId
    Guid roleId = roleName switch
    {
        "Leader" => Guid.Parse("19C40377-92B4-4E7E-B51B-33BC73994834"),
        "Member" => Guid.Parse("CCCD3629-1AD8-43B8-9CE7-B8268F7722A6"),
        _ => throw new Exception($"Invalid role '{roleName}'")
    };

    // 直接更新 User 的 RoleId
    user.RoleId = roleId;

    await _userRepository.UpdateAsync(user);
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




//         public async Task<List<DispalyTeamMemberDto>>SearchUersAsync(string keyword)
//             {
//                 if (string.IsNullOrEmpty(keyword))
//                 return new List<DispalyTeamMemberDto>();  // 空列表返回，不抛异常也可以

//                 var users = await _userRepository.GetAllAsync();

//                 var filtered = users.Where(u =>
//                          (u.Name != null && u.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
//                          (u.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
//                          u.UserRoles.Any(ur => ur.Role.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
//                                          ).ToList();

//                  return filtered.Select(u => new DispalyTeamMemberDto
//                         {
//                             Name = u.Name ?? "Unknown",
//                             Email = u.Email,
//                             Role = u.UserRoles.FirstOrDefault()?.Role.Name
//                         }).ToList();
// }




        }

    }

