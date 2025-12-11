using ProjectManagement.Domain.Entities.Collaborations;

namespace ProjectManagement.Domain.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(Guid id);
        Task<Comment?> GetByIdWithIncludesAsync(Guid id);  // 新增
        Task<List<Comment>> GetAllAsync();
        Task<List<Comment>> GetAllWithIncludesAsync();     // 新增
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(Comment comment);
    }
}
