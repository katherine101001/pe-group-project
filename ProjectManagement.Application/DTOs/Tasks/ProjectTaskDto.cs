namespace ProjectManagement.Application.DTOs.Tasks
{
    public enum TaskStatus
    {
        Todo,
        InProgress,
        Done
    }
    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }

    public enum TaskType
    {
        Feature,
        Bug,
        Improvement,
        Feature,
        Other
    }
    public class ProjectTaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = "";
        public TaskType Type {get;set;} 
        public TaskPriority Priority { get; set; }
        public string? AssigneeEmail { get; set; }
        public string Status { get; set; } 
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }        
        public DateTime UpdatedAt { get; set; }


        public class CreateTaskDto
    {
        public string Title { get; set; } = null!;     
        public string Description { get; set; } = ""; 
        public TaskType Type { get; set; }             
        public TaskPriority Priority { get; set; }     
        public string? AssigneeEmail { get; set; }     
        public TaskStatus Status { get; set; }         
        public DateTime? DueDate { get; set; }         
    }

    
    }
}
