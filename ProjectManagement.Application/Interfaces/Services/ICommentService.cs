using ProjectManagement.Application.DTOs.Collaboration;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface ICommentService
    {
        Task<CommentDto> AddCommentAsync(CommentDto dto);
        Task<CommentDto?> GetCommentByIdAsync(Guid id);
        Task<List<CommentDto>> GetAllCommentsAsync();
        Task UpdateCommentAsync(Guid id, CommentDto dto);
        Task DeleteCommentAsync(Guid id);
    }
}
