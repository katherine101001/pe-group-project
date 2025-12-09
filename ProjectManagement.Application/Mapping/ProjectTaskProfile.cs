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
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            // Input DTO -> Entity
            //CreateMap<CreateTaskDto, Task>();
            CreateMap<CreateProjectTaskDto, ProjectTask>();
        }
    }
}