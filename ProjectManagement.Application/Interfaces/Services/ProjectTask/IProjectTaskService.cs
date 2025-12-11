using ProjectManagement.Application.DTOs.Tasks;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface IProjectTaskService
    {
        Task <ProjectTaskDto> CreateProjectTaskAsync(CreateProjectTaskDto dto);
        Task<ProjectTaskDto?> GetProjectTaskByIdAsync(Guid id);
        Task<List<ProjectTaskDto>> GetAllProjectTasksAsync();
        Task UpdateProjectTaskAsync(Guid id, ProjectTaskDto dto);
        Task DeleteProjectTaskAsync(Guid id);


        Task<ProjectTaskDto> GetUpdateProjectTaskByIdAsync(Guid id);
        Task<ProjectTaskDetailsDto?> GetProjectTaskBrieflyByIdAsync(Guid id);
        Task<List<ProjectTaskDetailsDto>> GetAllProjectTasksBrieflyAsync();
   
    }
}
