using AutoMapper;

public class TeamMemberMappingProfile : Profile
{
    public TeamMemberMappingProfile()
    {
        CreateMap<TeamMember, TeamMemberDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
    }
}
