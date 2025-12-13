
using ProjectManagement.Domain.Entities.ProjectTasks;

namespace ProjectManagement.Domain.Interfaces.Repositories
{
    public interface IProjectTaskRepository
    {
        Task<ProjectTask?> GetByIdAsync(Guid id);
        Task<List<ProjectTask>> GetAllAsync();



        Task<int> CountTasksAsync(Guid projectId);
        Task<int> CountTasksByStatusAsync(Guid projectId, string status);
        Task<int> CountTasksByStatusesAsync(Guid projectId, IEnumerable<string> statuses);
        Task<int> CountOverdueTasksAsync(Guid projectId);

        Task<Dictionary<string, int>> GetTaskCountsGroupedByStatusAsync(Guid projectId);
        Task<Dictionary<string, int>> GetTaskCountsGroupedByTypeAsync(Guid projectId);
        Task<Dictionary<string, int>> GetTaskCountsGroupedByPriorityAsync(Guid projectId);



        Task AddAsync(ProjectTask projectTask);
        Task UpdateAsync(ProjectTask projectTask);
        Task DeleteAsync(ProjectTask projectTask);

        Task<int> GetTotalTasksAsync();

         Task<List<ProjectTask>> SearchAsync(string keyword);

         Task<int> GetMyTasksCountAsync(Guid userId);
         Task<int> GetOverdueTasksCountAsync(Guid userId);

    }
}
