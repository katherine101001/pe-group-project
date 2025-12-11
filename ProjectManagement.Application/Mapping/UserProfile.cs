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
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Role, opt => opt.Ignore());
            CreateMap<InviteTeamDto, User>();
            CreateMap<InviteTeamDto, User>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore()) // RoleId 需要在 Service 中手动设置
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => "New User")) // 可以给默认名字
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => "12345")); // 默认密码

            //CreateMap<UpdateUserDto, User>();

        }
    }
}