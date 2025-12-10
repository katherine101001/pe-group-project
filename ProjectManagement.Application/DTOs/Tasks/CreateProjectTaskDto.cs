using System;

namespace ProjectManagement.Application.DTOs.Tasks
{
    public class CreateProjectTaskDto
    {
        public Guid ProjectId { get; set; }          // Which project this task belongs to
        public Guid AssignToUserId { get; set; }          // User assigned to task

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string? Type { get; set; }            // Optional type field if needed
        public string? Status { get; set; } = "TO_DO";   // Default
        public string? Priority { get; set; } = "MEDIUM"; // Default

        public DateTime? DueDate { get; set; }
    }
}
