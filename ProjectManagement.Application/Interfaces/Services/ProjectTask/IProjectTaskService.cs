using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Application.DTOs.Projects;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface IProjectTaskService
    {
        Task <ProjectTaskDto> CreateProjectTaskAsync(CreateProjectTaskDto dto);
        Task<ProjectTaskDto?> GetProjectTaskByIdAsync(Guid id);
        Task<List<ProjectTaskDto>> GetAllProjectTasksAsync();
        Task UpdateProjectTaskAsync(Guid id, ProjectTaskDto dto);
        Task DeleteProjectTaskAsync(Guid id);


        Task<ProjectTaskDetails?> GetProjectTaskBrieflyByIdAsync(Guid id);
        Task<List<ProjectTaskDetails>> GetAllProjectTasksBrieflyAsync();

        Task<List<SearchTaskDto>> SearchTasksAsync(string keyword);
    
   
    }
}
