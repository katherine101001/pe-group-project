using AutoMapper;
using ProjectManagement.Application.DTOs.Collaboration;
using ProjectManagement.Domain.Entities.Collaborations;

public class CollaborationProfile : Profile
{
    public CollaborationProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Mentions, opt => opt.MapFrom(src => src.Mentions));

        CreateMap<Mention, MentionDto>()
            .ForMember(dest => dest.MentionedUserName, opt => opt.MapFrom(src => src.MentionedUser.Name));

        // Input DTO -> Entity
        CreateMap<CreateCommentDto, Comment>()
            .ForMember(dest => dest.ProjectTaskId, opt => opt.MapFrom(src => src.ProjectTaskId));
    }
}
