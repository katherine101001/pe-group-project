using System;

namespace ProjectManagement.Application.DTOs.Tasks
{
    //Overview of overdue tasks (ALL)
    public class OverdueTaskDto
    {
        public Guid ProjectId { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string? Type { get; set; }            // Optional type field if needed
        public DateTime? DueDate { get; set; }
    }
}
