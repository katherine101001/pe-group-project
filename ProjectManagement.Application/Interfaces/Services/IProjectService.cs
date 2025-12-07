using ProjectManagement.Application.DTOs.Projects;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task<ProjectDto> CreateProjectAsync(ProjectDto dto);
        Task<ProjectDto?> GetProjectByIdAsync(Guid id);
        Task<List<ProjectDto>> GetAllProjectsAsync();
        Task UpdateProjectAsync(Guid id, ProjectDto dto);
        Task DeleteProjectAsync(Guid id);
    }
}
