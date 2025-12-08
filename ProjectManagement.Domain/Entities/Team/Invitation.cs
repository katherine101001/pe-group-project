using System;
using System.Collections.Generic;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Domain.Entities.ProjectTasks;
using ProjectManagement.Domain.Entities.Collaborations;
using ProjectManagement.Domain.Entities.Projects;

public class Invitation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid TeamId { get; set; }          // 所属团队
    public string Email { get; set; }         // 被邀请人的邮箱
    public string Role { get; set; }          // 被邀请人的角色
    public bool IsAccepted { get; set; } = false;  // 是否接受邀请
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
