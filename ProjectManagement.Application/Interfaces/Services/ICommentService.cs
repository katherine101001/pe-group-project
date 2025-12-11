using ProjectManagement.Application.DTOs.Collaboration;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface ICommentService
    {
        Task<CommentDto> AddCommentAsync(CreateCommentDto dto);
        Task<CommentDto?> GetCommentByIdAsync(Guid id);
        Task<List<CommentDto>> GetAllCommentsAsync();
        Task<CommentDto> UpdateCommentAsync(Guid id, string content);        
        Task DeleteCommentAsync(Guid id);
    }
}
