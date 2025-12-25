using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using ProjectManagement.Domain.Entities.Collaborations;
using ProjectManagement.Domain.Entities.Projects;
using ProjectManagement.Domain.Entities.ProjectTasks;

namespace ProjectManagement.Domain.Entities.Users
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? ProfilePicture { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public Guid RoleId { get; set; }

        public Role Role { get; set; } = null!;
        
        public bool IsActivated { get; set; } = false;
        public ICollection<Project> LeadingProjects { get; set; } = new List<Project>();

        public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
        public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
        public ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();
        public ICollection<TaskAttachment> Attachments { get; set; } = new List<TaskAttachment>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Mention> Mentions { get; set; } = new List<Mention>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
