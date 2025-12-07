
using ProjectManagement.Domain.Entities.Tasks;

namespace ProjectManagement.Application.Interfaces.Repositories
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
