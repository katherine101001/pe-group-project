using ProjectManagement.Application.DTOs.Projects;
using ProjectManagement.Application.DTOs.Tasks;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task CreateProjectAsync(CreateProjectDto dto);
        Task<ProjectOverviewDto?> GetProjectByIdAsync(Guid id);
        Task<List<ProjectDto>> GetAllProjectsAsync();
        Task UpdateProjectAsync(Guid id, ProjectDto dto);
        Task DeleteProjectAsync(Guid id);

    }
}
