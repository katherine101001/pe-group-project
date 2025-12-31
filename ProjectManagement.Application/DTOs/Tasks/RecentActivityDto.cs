using System;
using ProjectManagement.Domain.Entities.Users;

namespace ProjectManagement.Application.DTOs.Tasks
{
    public class RecentActivityDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public string? Type { get; set; }
        public string Status { get; set; } = "TO_DO";

        public Guid AssignToUserId { get; set; }
        // public User AssignToUser { get; set; } = null!;

        public string? AssigneeName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
