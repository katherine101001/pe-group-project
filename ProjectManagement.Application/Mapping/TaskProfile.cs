using AutoMapper;
using ProjectManagement.Domain.Entities.Tasks;
using ProjectManagement.Application.DTOs.Tasks;

public class ProjectTaskProfile : Profile
{
    public ProjectTaskProfile()
    {
        // Entity -> Output DTO
       CreateMap<CreateTaskDto, ProjectTask>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        
        // Input DTO -> Entity
        //CreateMap<CreateTaskDto, Task>();
        CreateMap<ProjectTask, ProjectTaskDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<TaskType>(src.Type)))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => Enum.Parse<TaskPriority>(src.Priority)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TaskStatus>(src.Status)));
    }
}
