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

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            // Map input DTO -> Entity
            var user = _mapper.Map<User>(dto);

            await _userRepository.AddAsync(user);

            // Map Entity -> DTO to return
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

        public async Task<UserDto?> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return null;

            if (user.Password != password)
                return null;

            return _mapper.Map<UserDto>(user);
        }

        public async Task InviteUserAsync(InviteTeamDto dto)
        {

            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
             {
                 throw new Exception($"User with email '{dto.Email}' not found");
             }

            var roleName = string.IsNullOrEmpty(dto.Role) ? "Member" : dto.Role;

            Guid roleId = dto.Role switch
            {
                 "Admin" => Guid.Parse("11111111-1111-1111-1111-111111111111"),
                 "Member" => Guid.Parse("22222222-2222-2222-2222-222222222222"),
                _ => throw new Exception($"Invalid role '{dto.Role}'")
            };

            user.UserRoles.Clear();

             user.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = roleId
            });

            await _userRepository.UpdateAsync(user);
        }

        public async Task<List<DispalyTeamMemberDto>> GetAllUsersSimpleAsync()
        {
           var users = await _userRepository.GetAllAsync();
           return users.Select(u => new DispalyTeamMemberDto
            {
                 Name = u.Name ?? "Unknown",
                 Email = u.Email,
                 Role = u.UserRoles.FirstOrDefault()?.Role.Name ?? "Member"
 
            }).ToList();
        }



        }

    }

