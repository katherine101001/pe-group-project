
using ProjectManagement.Domain.Entities.Collaborations;
using ProjectManagement.Domain.Entities.ProjectTasks;

namespace ProjectManagement.Domain.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(Guid id);
        Task<List<Comment>> GetAllAsync();
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(Comment comment);
    }
}
