
using ProjectManagement.Domain.Entities.Collaborations;

namespace ProjectManagement.Application.Interfaces.Repositories
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
