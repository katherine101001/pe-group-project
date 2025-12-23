using System;
using System.Collections.Generic;
using ProjectManagement.Domain.Entities.Users;

namespace ProjectManagement.Domain.Entities.Projects
{
    public class ProjectMember
    {
        // Foreign key to Project
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }  // Navigation property

        // Foreign key to User
        public Guid UserId { get; set; }
        public User? User { get; set; }        // Navigation property

        // Optional: additional info for membership
        public string? RoleInProject { get; set; } = "Member";
        public DateTime? JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
