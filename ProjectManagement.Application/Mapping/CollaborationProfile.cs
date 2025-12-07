using AutoMapper;
using ProjectManagement.Domain.Entities.Collaborations;
using ProjectManagement.Application.DTOs.Collaboration;

public class CollaborationProfile : Profile
{
    public CollaborationProfile()
    {
        // Entity -> Output DTO
        //CreateMap<Comment, CommentDto>();

        // Input DTO -> Entity
        //CreateMap<CreateCommentDto, Comment>();
    }
}
