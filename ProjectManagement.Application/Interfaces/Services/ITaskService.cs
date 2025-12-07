using ProjectManagement.Application.DTOs.Tasks;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(TaskDto dto);
        Task<TaskDto?> GetTaskByIdAsync(Guid id);
        Task<List<TaskDto>> GetAllTasksAsync();
        Task UpdateTaskAsync(Guid id, TaskDto dto);
        Task DeleteTaskAsync(Guid id);
    }
}
