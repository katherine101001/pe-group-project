namespace ProjectManagement.Application.DTOs.Collaboration
{
    public class MentionDto
    {
        public Guid Id { get; set; }
        public Guid CommentId { get; set; }

        public Guid MentionedUserId { get; set; }
        public string MentionedUserName { get; set; } = string.Empty; 
        public bool? Read { get; set; }                             
    }
}
