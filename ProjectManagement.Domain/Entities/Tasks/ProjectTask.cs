using System;
using System.Collections.Generic;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Domain.Entities.Projects;
using ProjectManagement.Domain.Entities.Collaborations;


namespace ProjectManagement.Domain.Entities.Tasks
{
    public class ProjectTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = "";
        public string Type { get; set; } = "Feature";       
        public string Priority { get; set; } = "Medium";    
        public string? AssigneeEmail { get; set; }          
        public string Status { get; set; } = "Todo";        
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<SubTask> Tasks { get; set; } = new List<SubTask>();
        public ICollection<TaskAttachment> Attachments { get; set; } = new List<TaskAttachment>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
