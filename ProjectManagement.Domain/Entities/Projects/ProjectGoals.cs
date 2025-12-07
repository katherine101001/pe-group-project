using System;
using System.Collections.Generic;

using ProjectManagement.Domain.Entities.Projects;

namespace ProjectManagement.Domain.Entities
{
    public class ProjectGoal
    {
        public Guid Id { get; set; }                 // Primary key
        public Guid ProjectId { get; set; }          // Foreign key
        public Project Project { get; set; } = null!; // Navigation to Project

        public string? Title { get; set; }    // Goal title
        public string? Description { get; set; } = ""; // Optional description
        public DateTime? DueDate { get; set; }        // Goal deadline
        public bool? IsCompleted { get; set; } = false;

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
