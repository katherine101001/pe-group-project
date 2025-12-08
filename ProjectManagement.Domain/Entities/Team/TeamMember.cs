using System;
using System.Collections.Generic;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Domain.Entities.ProjectTasks;
using ProjectManagement.Domain.Entities.Collaborations;
using ProjectManagement.Domain.Entities.Projects;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}


public class TeamMember
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
    public string Role { get; set; } = null!;

    public User User { get; set; } = null!;
}

// Invitation 实体
public class Invitation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string InvitedEmail { get; set; } = null!;
    public string TargetRole { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(7);
}
