namespace ProjectManagement.Application.DTOs.Projects
{
    public class ProjectOverviewDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "MEDIUM";
        public string Status { get; set; } = "PLANNING";
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int TotalTeamMembers { get; set; }
    }
}
