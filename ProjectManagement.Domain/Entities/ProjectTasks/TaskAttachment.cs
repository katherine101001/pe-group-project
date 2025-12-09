using System;
using System.Collections.Generic;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Domain.Entities.Projects;


namespace ProjectManagement.Domain.Entities.ProjectTasks
{
    public class TaskAttachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();                // Primary key

        public Guid TaskId { get; set; }          // Foreign key
        public ProjectTask ProjectTask { get; set; } = null!; // Navigation to ProjectTask


        public Guid? UploadedBy { get; set; }
        public User? User { get; set; }

        public string? Filename { get; set; }
        public string? FileURL { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();

    }
}
