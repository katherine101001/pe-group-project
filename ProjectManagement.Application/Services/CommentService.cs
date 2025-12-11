// using AutoMapper;
// using ProjectManagement.Application.DTOs.Collaboration;
// using ProjectManagement.Domain.Interfaces.Repositories;
// using ProjectManagement.Application.Interfaces.Services;
// using ProjectManagement.Domain.Entities.Collaborations;
// using ProjectManagement.Shared.Exceptions;

// namespace ProjectManagement.Application.Services
// {
//     public class CommentService : ICommentService
//     {
//         private readonly ICommentRepository _commentRepository;
//         private readonly IMapper _mapper;

//         public CommentService(ICommentRepository commentRepository, IMapper mapper)
//         {
//             _commentRepository = commentRepository;
//             _mapper = mapper;
//         }

//         public async Task<CommentDto> AddCommentAsync(CommentDto dto)
//         {
//             // Map DTO -> Entity
//             var comment = _mapper.Map<Comment>(dto);

//             // Save entity
//             await _commentRepository.AddAsync(comment);

//             // Map Entity -> DTO to return
//             return _mapper.Map<CommentDto>(comment);
//         }

//         public async Task<CommentDto?> GetCommentByIdAsync(Guid id)
//         {
//             var comment = await _commentRepository.GetByIdAsync(id);

//             if (comment == null)
//                 return null;

//             return _mapper.Map<CommentDto>(comment);
//         }

//         public async Task<List<CommentDto>> GetAllCommentsAsync()
//         {
//             var comments = await _commentRepository.GetAllAsync();
//             return _mapper.Map<List<CommentDto>>(comments);
//         }

//         public async Task UpdateCommentAsync(Guid id, CommentDto dto)
//         {
//             var existingComment = await _commentRepository.GetByIdAsync(id);

//             if (existingComment == null)
//                 throw new NotFoundException("Comment not found");

//             // Map updated values from DTO to entity
//             _mapper.Map(dto, existingComment);

//             await _commentRepository.UpdateAsync(existingComment);
//         }

//         public async Task DeleteCommentAsync(Guid id)
//         {
//             var existingComment = await _commentRepository.GetByIdAsync(id);

//             if (existingComment == null)
//                 throw new NotFoundException("Comment not found");

//             await _commentRepository.DeleteAsync(existingComment);
//         }
//     }
// }

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.DTOs.Collaboration;
using ProjectManagement.Application.Interfaces.Services;
using ProjectManagement.Domain.Entities.Collaborations;
using ProjectManagement.Domain.Interfaces.Repositories;

namespace ProjectManagement.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public CommentService(
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IEmailService emailService,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _mapper = mapper;
        }

       public async Task<CommentDto> AddCommentAsync(CreateCommentDto dto)
        {
            // 创建 Comment
            var comment = _mapper.Map<Comment>(dto);

            // 先添加 Mention
            if (dto.MentionedUserIds != null)
            {
                foreach (var userId in dto.MentionedUserIds)
                {
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user != null)
                    {
                        comment.Mentions.Add(new Mention
                        {
                            CommentId = comment.Id,
                            MentionedUserId = userId
                        });
                    }
                }
            }

            // 保存 Comment（包含 Mentions）
            await _commentRepository.AddAsync(comment);

            // 查询完整 Comment
            var fullComment = await _commentRepository.GetByIdWithIncludesAsync(comment.Id);
            var commentDto = _mapper.Map<CommentDto>(fullComment);

            // 发送邮件（异步，不影响保存）
            if (dto.MentionedUserIds != null)
            {
                foreach (var userId in dto.MentionedUserIds)
                {
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user != null)
                    {
                        await _emailService.SendMentionEmailAsync(
                            user.Email,
                            comment.Content ?? "",
                            user.Name ?? "User"
                        );
                    }
                }
            }

            return commentDto;
        }

        public async Task<CommentDto?> GetCommentByIdAsync(Guid id)
        {
            var comment = await _commentRepository.GetByIdWithIncludesAsync(id);
            return comment == null ? null : _mapper.Map<CommentDto>(comment);
        }

        public async Task<List<CommentDto>> GetAllCommentsAsync()
        {
            var comments = await _commentRepository.GetAllWithIncludesAsync();
            return _mapper.Map<List<CommentDto>>(comments);
        }

        public async Task<CommentDto> UpdateCommentAsync(Guid id, string content)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) throw new Exception("Comment not found");

            comment.Content = content;
            comment.UpdatedAt = DateTime.UtcNow;

            await _commentRepository.UpdateAsync(comment);

            var updatedComment = await _commentRepository.GetByIdWithIncludesAsync(id);
            return _mapper.Map<CommentDto>(updatedComment);
        }

        public async Task DeleteCommentAsync(Guid id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) throw new Exception("Comment not found");

            await _commentRepository.DeleteAsync(comment);
        }
    }
}
