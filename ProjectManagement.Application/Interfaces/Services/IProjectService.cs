using ProjectManagement.Application.DTOs.Projects;
using ProjectManagement.Application.DTOs.Tasks;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task CreateProjectAsync(CreateProjectDto dto);
        Task<ProjectOverviewDto?> GetProjectByIdAsync(Guid id);
        Task<List<ProjectDto>> GetAllProjectsAsync();

        // To fetch the details from DB to form to let user update the form without needing to refill the form
        Task<GetUpdateProjectDto> GetUpdateProjectByIdAsync(Guid id);
        Task UpdateProjectAsync(Guid id, UpdateProjectDto dto);


        Task DeleteProjectAsync(Guid id);

        Task<List<SearchProjectDto>> SearchProjectsAsync(string keyword);



    }
}
