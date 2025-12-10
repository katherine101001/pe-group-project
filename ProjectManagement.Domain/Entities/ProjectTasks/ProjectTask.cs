using System;
using System.Collections.Generic;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Domain.Entities.Projects;
using ProjectManagement.Domain.Entities.Collaborations;


namespace ProjectManagement.Domain.Entities.ProjectTasks
{
    public class ProjectTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();                // Primary key

        public Guid ProjectId { get; set; }          // Foreign key
        public Project Project { get; set; } = null!; // Navigation to Project


        public Guid AssignToUserId { get; set; }
        public User AssignToUser { get; set; } = null!;

        public string? Title { get; set; }    // Goal title
        public string? Description { get; set; } = ""; // Optional description
        public DateTime? DueDate { get; set; }        // Goal deadline
        public string? Status { get; set; }
        public string? Priority { get; set; }

        public string? Type { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();
        public ICollection<TaskAttachment> Attachments { get; set; } = new List<TaskAttachment>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
