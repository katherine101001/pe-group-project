using ProjectManagement.Domain.Entities.Collaborations;

namespace ProjectManagement.Domain.Interfaces.Repositories
{
    public interface IMentionRepository
    {
        Task<List<Mention>> GetByCommentIdAsync(Guid commentId);
        Task AddAsync(Mention mention);
        Task DeleteAsync(Mention mention);
    }
}
