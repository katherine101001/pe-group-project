using AutoMapper;
using ProjectManagement.Domain.Entities.ProjectTasks;
using ProjectManagement.Application.DTOs.Tasks;

namespace ProjectManagement.Application.Mapping
{
    public class ProjectTaskProfile : Profile
    {
        public ProjectTaskProfile()
        {
            // Entity -> Output DTO
             CreateMap<ProjectTask, ProjectTaskDto>()
                .ForMember(dest => dest.AssigneeName, opt => opt.MapFrom(src => src.AssignToUser.Name));
    

            CreateMap<ProjectTask, ProjectTaskDetailsDto>()
                .ForMember(dest => dest.AssigneeName, opt => opt.MapFrom(src => src.AssignToUser.Name));

            // Input DTO -> Entity
            //CreateMap<CreateTaskDto, Task>();
            CreateMap<CreateProjectTaskDto, ProjectTask>();
        }
    }
}