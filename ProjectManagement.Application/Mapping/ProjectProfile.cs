using AutoMapper;
using ProjectManagement.Domain.Entities.Projects;
using ProjectManagement.Application.DTOs.Projects;

namespace ProjectManagement.Application.Mapping
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            // Entity -> Output DTO
            CreateMap<Project, ProjectDto>();
            CreateMap<Project, ProjectOverviewDto>();
            CreateMap<Project, GetUpdateProjectDto>();

            // Input DTO -> Entity
            CreateMap<CreateProjectDto, Project>();
            CreateMap<UpdateProjectDto, Project>();

            CreateMap<Project, SearchProjectDto>();
        }
    }
}