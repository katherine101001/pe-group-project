using AutoMapper;
using ProjectManagement.Domain.Entities.ProjectTasks;
using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Application.DTOs;

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

            CreateMap<ProjectTask, SearchTaskDto>();

            CreateMap<ProjectTask, ProjectTaskCalendarDto>()
                .ForMember(dest => dest.Date,
                        opt => opt.MapFrom(src => src.DueDate.HasValue ? src.DueDate.Value.ToString("yyyy-MM-dd") : ""))
                .ForMember(dest => dest.TaskCount, opt => opt.Ignore());

            CreateMap<ProjectTask, OverdueTaskDto>();
        }
    }
}