namespace ProjectManagement.Application.DTOs.Collaboration
{
    public class CreateCommentDto
    {
       public Guid ProjectTaskId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = null!;
        public List<Guid>? MentionedUserIds { get; set; } = new();
    }
}
