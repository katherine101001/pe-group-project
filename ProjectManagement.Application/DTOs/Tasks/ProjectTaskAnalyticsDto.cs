namespace ProjectManagement.Application.DTOs.Tasks
{
    public class ProjectTaskAnalyticsDto
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int ActiveTasks { get; set; }
        public int OverdueTasks { get; set; }
        public int TeamSize { get; set; }

        public double CompletionRate { get; set; }  // Business logic will be added here

        public Dictionary<string, int> TasksByStatus { get; set; } = new();
        public Dictionary<string, int> TasksByType { get; set; } = new();
        public Dictionary<string, int> TasksByPriority { get; set; } = new();
    }

}
