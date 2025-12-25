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

            CreateMap<LoginDto, User>()
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            //CreateMap<UpdateUserDto, User>();

            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.NewPassword)) // 使用新密码更新
                .ForMember(dest => dest.RoleId, opt => opt.Ignore()) // Service 里设置 RoleId
                .ForMember(dest => dest.Role, opt => opt.Ignore())   // Service 里设置 Role 对象
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // Service 更新
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // 不改


        }
    }
}