using System;
using System.Collections.Generic;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Domain.Entities.Projects;


namespace ProjectManagement.Domain.Entities.ProjectTasks
{
    public class SubTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();             // Primary key

        public Guid ProjectTaskId { get; set; }          // Foreign key
        public ProjectTask ProjectTask { get; set; } = null!; // Navigation to ProjectTask


        public Guid? AssignToUserId { get; set; }
        public User? AssignToUser { get; set; }

        public string? Title { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<SubTask> Tasks { get; set; } = new List<SubTask>();

    }
}
