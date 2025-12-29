namespace ProjectManagement.Application.DTOs.Projects
{
    public class ProjectOverviewDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "MEDIUM";
        public string Status { get; set; } = "PLANNING";
        public bool IsArchived { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int TotalTeamMembers { get; set; }
        public int Progress { get; set; }

        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "MEDIUM";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string> TeamMemberEmails { get; set; } = new();
        public string TeamLeadEmail { get; set; } = string.Empty;
    }
}
