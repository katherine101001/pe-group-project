using System;
using System.Collections.Generic;

namespace ProjectManagement.Application.DTOs.Collaboration
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid ProjectTaskId { get; set; }
        public Guid UserId { get; set; }
         public string UserName { get; set; } = string.Empty; 
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
        public List<MentionDto> Mentions { get; set; } = new(); 
    }
}
