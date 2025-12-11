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
            CreateMap<User,DisplayTeamMemberDto>()
                      .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : "Member"));
            // CreateMap<User,InviteTeamDto>();

            // Input DTO -> Entity
            CreateMap<CreateUserDto, User>();
            //    .ForMember(dest => dest.Role, opt => opt.Ignore());
            CreateMap<InviteTeamDto, User>();
            //CreateMap<UpdateUserDto, User>();

        }
    }
}