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
        private readonly IMentionRepository _mentionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

       public CommentService(
        ICommentRepository commentRepository,
        IMentionRepository mentionRepository,
        IUserRepository userRepository,
        IEmailService emailService,
        IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mentionRepository = mentionRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _mapper = mapper;
        }
public async Task<CommentDto> AddCommentAsync(CreateCommentDto dto)
{
    if (dto == null) throw new ArgumentNullException(nameof(dto));

    // 1️⃣ 创建 Comment 实体
    var comment = _mapper.Map<Comment>(dto);

    // 2️⃣ 保存 Comment 生成 Id
    await _commentRepository.AddAsync(comment);

    // 3️⃣ 添加 Mentions
    if (dto.MentionedUserIds != null && dto.MentionedUserIds.Any())
    {
        foreach (var userId in dto.MentionedUserIds)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                var mention = new Mention
                {
                    CommentId = comment.Id,
                    MentionedUserId = userId,
                    MentionedUser = user
                };
                await _mentionRepository.AddAsync(mention); // ⚡ 单独保存
            }
        }
    }

    // 4️⃣ 重新获取 Comment（包含 Mentions & User）
    var fullComment = await _commentRepository.GetByIdWithIncludesAsync(comment.Id);
    var commentDto = _mapper.Map<CommentDto>(fullComment);

    // 5️⃣ 发 Mention 邮件
    if (dto.MentionedUserIds != null && dto.MentionedUserIds.Any())
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
