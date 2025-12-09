using System;
using System.Collections.Generic;
using ProjectManagement.Domain.Entities.ProjectTasks;
using ProjectManagement.Domain.Entities.Users;

namespace ProjectManagement.Domain.Entities.Collaborations
{
    public class Mention
    {
        public Guid Id { get; set; } = Guid.NewGuid();                // Primary key

        public Guid CommentId { get; set; }          // Foreign key
        public Comment Comment { get; set; } = null!; // Navigation to Comment

        public Guid MentionedUserId { get; set; }          // Foreign key
        public User User { get; set; } = null!; // Navigation to User

        public bool? Read { get; set; }


    }
}
