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

        // 计算用户被指派的任务数量
private async Task<int> GetTotalTasksByUserAsync(Guid userId)
{
    // 获取所有项目
    var projects = await _projectRepository.GetAllAsync();

    // 找出用户参与的项目（leader 或成员）
    var userProjects = projects.Where(p =>
        p.LeaderId == userId || p.ProjectMembers.Any(pm => pm.UserId == userId)
    );

    // 累加用户在这些项目里被指派的任务数量
    int totalTasks = userProjects.Sum(p =>
        p.ProjectTasks.Count(t => t.AssignToUserId == userId)
    );

    return totalTasks;
}


        public async Task DeleteUserAsync(Guid userId)
{
    // 1️⃣ 检查用户是否参与任何项目
    var totalProjects = await _projectRepository.GetTotalProjectsByUserAsync(userId);

    // 2️⃣ 检查用户是否有指派的任务
    var totalTasks = await GetTotalTasksByUserAsync(userId);

    // 3️⃣ 如果有项目或任务，阻止删除
    if (totalProjects > 0 || totalTasks > 0)
    {
        throw new InvalidOperationException(
            $"Cannot delete user: assigned to {totalProjects} project(s)."
        );
    }

    // 4️⃣ 获取用户实体并删除
    var user = await _userRepository.GetByIdAsync(userId);
    if (user == null)
        throw new NotFoundException("User not found.");

    await _userRepository.DeleteAsync(user);
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

    // 🔒 如果未激活，直接返回 null
    if (!user.IsActivated)
        throw new Exception("Account not activated. Please complete registration.");

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



public async Task<UserDto> RegisterAsync(RegisterDto dto)
{
    // 1. 查找邀请用户
    var user = await _userRepository.GetByEmailAsync(dto.Email);
    if (user == null)
        throw new Exception("Invalid invitation email.");

    // 2. 检查是否已经激活
    if (user.IsActivated)
        throw new Exception("User already activated.");

    // 3. 验证 invitation 密码
    if (user.Password != dto.Password)
        throw new Exception("Invalid invitation password.");

    // 4. 验证新密码确认
    if (dto.NewPassword != dto.ConfirmPassword)
        throw new Exception("New password and confirm password do not match.");

    // 5. 更新姓名和密码，同时标记激活
    user.Name = dto.Name;
    user.Password = dto.NewPassword;
    user.IsActivated = true;
    user.UpdatedAt = DateTime.UtcNow;

    // 6. 保存更新
    await _userRepository.UpdateAsync(user);

    return _mapper.Map<UserDto>(user);
}











    }



}

