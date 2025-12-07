
using ProjectManagement.Domain.Entities.ProjectTasks;

namespace ProjectManagement.Domain.Interfaces.Repositories
{
    public interface IProjectTaskRepository
    {
        Task<ProjectTask?> GetByIdAsync(Guid id);
        Task<List<ProjectTask>> GetAllAsync();
        Task AddAsync(ProjectTask projectTask);
        Task UpdateAsync(ProjectTask projectTask);
        Task DeleteAsync(ProjectTask projectTask);
    }
}
