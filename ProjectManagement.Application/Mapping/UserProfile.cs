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
            .ForMember(dest => dest.RoleId, opt => opt.Ignore()) // 由 Service 设置 RoleId
            .ForMember(dest => dest.Role, opt => opt.Ignore())   // 忽略 Role 对象
            .ForMember(dest => dest.Name, opt => opt.Ignore())   // 默认名字在 Service 设置
            .ForMember(dest => dest.Password, opt => opt.Ignore()); // 默认密码在 Service 设置

            //CreateMap<UpdateUserDto, User>();

        }
    }
}