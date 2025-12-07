using AutoMapper;
using ProjectManagement.Application.DTOs.Collaboration;
using ProjectManagement.Application.Interfaces.Repositories;
using ProjectManagement.Application.Interfaces.Services;
using ProjectManagement.Domain.Entities.Collaborations;
using ProjectManagement.Shared.Exceptions;

namespace ProjectManagement.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<CommentDto> AddCommentAsync(CommentDto dto)
        {
            // Map DTO -> Entity
            var comment = _mapper.Map<Comment>(dto);

            // Save entity
            await _commentRepository.AddAsync(comment);

            // Map Entity -> DTO to return
            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto?> GetCommentByIdAsync(Guid id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
                return null;

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<List<CommentDto>> GetAllCommentsAsync()
        {
            var comments = await _commentRepository.GetAllAsync();
            return _mapper.Map<List<CommentDto>>(comments);
        }

        public async Task UpdateCommentAsync(Guid id, CommentDto dto)
        {
            var existingComment = await _commentRepository.GetByIdAsync(id);

            if (existingComment == null)
                throw new NotFoundException("Comment not found");

            // Map updated values from DTO to entity
            _mapper.Map(dto, existingComment);

            await _commentRepository.UpdateAsync(existingComment);
        }

        public async Task DeleteCommentAsync(Guid id)
        {
            var existingComment = await _commentRepository.GetByIdAsync(id);

            if (existingComment == null)
                throw new NotFoundException("Comment not found");

            await _commentRepository.DeleteAsync(existingComment);
        }
    }
}
