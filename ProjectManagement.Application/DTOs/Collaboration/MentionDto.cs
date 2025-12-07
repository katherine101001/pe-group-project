namespace ProjectManagement.Application.DTOs.Collaboration
{
    public class MentionDto
    {
        public Guid Id { get; set; }
        public Guid CommentId { get; set; }
        public Guid MentionedUserId { get; set; }
    }
}
