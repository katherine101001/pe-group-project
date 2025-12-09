using AutoMapper;
using ProjectManagement.Application.DTOs.Team;
using ProjectManagement.Domain.Entities.Team;

namespace ProjectManagement.Application.Mapping
{
    public class InvitationProfile : Profile
    {
        public InvitationProfile()
        {
            //CreateMap<InviteMemberDto, Invitation>();           // DTO -> Entity
            CreateMap<Invitation, InvitationResponseDto>();
        }
    }
}
