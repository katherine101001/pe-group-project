using ProjectManagement.Application.DTOs.Tasks;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface IProjectTaskService
    {
        Task<ProjectTaskDto> CreateProjectTaskAsync(CreateTaskDto dto); 
        Task<ProjectTaskDto?> GetProjectTaskByIdAsync(Guid id);
        Task<List<ProjectTaskDto>> GetAllProjectTasksAsync();
        Task UpdateProjectTaskAsync(Guid id, UpdateTaskDto dto);
        Task DeleteProjectTaskAsync(Guid id);
    }
}
