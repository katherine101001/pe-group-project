using System;
using ProjectManagement.Domain.Entities.Users;

namespace ProjectManagement.Application.DTOs.Tasks
{
    public class ProjectTaskDetailsDto
    {
        public string Title { get; set; } = null!;
        public string? Type { get; set; }
        public string Status { get; set; } = "TO_DO";
        public string Priority { get; set; } = "MEDIUM";
        public string Description { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public Guid AssignToUserId { get; set; }
        //        public User AssignToUser { get; set; } = null!; 
        public string? AssigneeName { get; set; }

        public DateTime? DueDate { get; set; }

    }
}
