using System;
using System.Collections.Generic;
using ProjectManagement.Domain.Entities.Tasks;
using ProjectManagement.Domain.Entities.Users;

namespace ProjectManagement.Domain.Entities.Collaborations
{
    public class Comment
    {
        public Guid Id { get; set; }                 // Primary key

        public Guid TaskId { get; set; }          // Foreign key
        public ProjectTask Task { get; set; } = null!; // Navigation to Task

        public Guid UserId { get; set; }          // Foreign key
        public User User { get; set; } = null!; // Navigation to Task

        public string? Content { get; set; } = ""; // Optional description


        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Mention> Mentions { get; set; } = new List<Mention>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
