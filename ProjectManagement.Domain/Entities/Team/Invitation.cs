using System;
using System.Collections.Generic;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Domain.Entities.ProjectTasks;
using ProjectManagement.Domain.Entities.Collaborations;
using ProjectManagement.Domain.Entities.Projects;

namespace ProjectManagement.Domain.Entities.Team
{
    public class Invitation
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string InvitedEmail { get; set; }
        public required string TargetRole { get; set; }

        public string Token { get; set; } = null!;
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(7);
    }
}
