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
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectTaskRepository _projectTaskRepository;

        private readonly IMapper _mapper;



        public UserService(IUserRepository userRepository, IMapper mapper, IProjectRepository projectRepository,
            IProjectTaskRepository projecttaskRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _projectRepository = projectRepository;
            _projectTaskRepository = projecttaskRepository;
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
            // 1. 根据邮箱查找用户
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

            // 2. 根据角色名称查找角色
            var role = await _userRepository.GetByNameAsync(dto.Role);
            if (role == null)
                throw new Exception($"Role '{dto.Role}' not found.");

            if (existingUser != null)
            {
                // 用户已存在，更新角色
                existingUser.RoleId = role.Id;
                await _userRepository.UpdateAsync(existingUser); // 假设 Add/Update 内部已经保存
                return existingUser;
            }

            // 3. 用户不存在，创建新用户
            var user = _mapper.Map<User>(dto);
            user.RoleId = role.Id;           // 给用户赋角色
            user.Name = "New User";          // 默认名字
            user.Password = "12345";         // 默认密码

            // 4. 保存新用户
            await _userRepository.AddAsync(user);  // 内部保存到数据库

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
        public async Task<DashboardTeam> GetDashboardTeamStatsAsync()
        {
            return new DashboardTeam
            {
                TotalUsers = await _userRepository.GetTotalUsersAsync(),
                TotalProjects = await _projectRepository.GetTotalProjectsAsync(),
                TotalTasks = await _projectTaskRepository.GetTotalTasksAsync()
            };
        }


        public async Task<User?> LoginAsync(LoginDto dto)
        {
            if (dto == null) return null;

            var user = await _userRepository.GetByEmailAsyncLogin(dto.Email);
            if (user == null) return null;

            if (user.Password != dto.Password) return null;

            if (user.Role == null) return null;
            if (!string.Equals(user.Role.Name, dto.Role, StringComparison.OrdinalIgnoreCase))
                return null;

            return user;
        }




        public async Task<List<AvailableUserDto>> GetAvailableMembersAsync(Guid projectId)
        {
            var users = await _userRepository.GetUsersNotInProjectAsync(projectId);

            return users.Select(u => new AvailableUserDto
            {
                Id = u.Id,
                Email = u.Email
            }).ToList();
        }


    }



}

