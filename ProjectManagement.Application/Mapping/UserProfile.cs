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

            // Input DTO -> Entity
            CreateMap<CreateUserDto, User>();
            //CreateMap<UpdateUserDto, User>();

            CreateMap<User,InviteTeamDto>();
            
            CreateMap<InviteTeamDto, User>();
        }
    }
}