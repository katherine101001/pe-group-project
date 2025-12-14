using ProjectManagement.Domain.Entities.Collaborations;

namespace ProjectManagement.Domain.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(Guid id);
        Task<Comment?> GetByIdWithIncludesAsync(Guid id);  
        Task<List<Comment>> GetAllAsync();
        Task<List<Comment>> GetAllWithIncludesAsync();     
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(Comment comment);
    }
}
