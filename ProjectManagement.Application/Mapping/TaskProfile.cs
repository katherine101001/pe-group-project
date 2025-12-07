using AutoMapper;
using ProjectManagement.Domain.Entities.Tasks;
using ProjectManagement.Application.DTOs.Tasks;

public class ProjectTaskProfile : Profile
{
    public ProjectTaskProfile()
    {
        // Entity -> Output DTO
        CreateMap<ProjectTask, ProjectTaskDto>();

        // Input DTO -> Entity
        //CreateMap<CreateTaskDto, Task>();
    }
}
