using System;
using System.Collections.Generic;
using ProjectManagement.Domain.Entities.ProjectTasks;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Domain.Entities.Projects;

namespace ProjectManagement.Domain.Entities.Collaborations
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public string? Content { get; set; }
        public bool? IsRead { get; set; } = false;



        // Nullable FKs for polymorphic references
        public Guid? TaskId { get; set; }
        public ProjectTask? ProjectTask { get; set; }

        public Guid? CommentId { get; set; }
        public Comment? Comment { get; set; }

        public Guid? ProjectId { get; set; }
        public Project? Project { get; set; }



        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
