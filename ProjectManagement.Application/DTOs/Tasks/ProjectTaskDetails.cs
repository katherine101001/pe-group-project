using System;
using ProjectManagement.Application.DTOs.Users;

namespace ProjectManagement.Application.DTOs.Tasks
{
    public class ProjectTaskDetails
    {
        public string Title { get; set; } = null!;
        public string? Type { get; set; }
        public string Status { get; set; } = "TO_DO";
        public string Priority { get; set; } = "MEDIUM";
        public string Description { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public Guid? AssignTo { get; set; }
        public UserDto? User { get; set; }

        public DateTime? DueDate { get; set; }

    }
}
