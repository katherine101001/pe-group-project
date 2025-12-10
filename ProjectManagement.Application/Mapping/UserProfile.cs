using AutoMapper;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Application.DTOs.Users;

namespace ProjectManagement.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Entity -> Output DTO
            CreateMap<User, UserDto>();
            CreateMap<User,DispalyTeamMemberDto>();
            CreateMap<User,InviteTeamDto>();

            // Input DTO -> Entity
            CreateMap<CreateUserDto, User>();
            CreateMap<InviteTeamDto, User>();
            //CreateMap<UpdateUserDto, User>();

        }
    }
}