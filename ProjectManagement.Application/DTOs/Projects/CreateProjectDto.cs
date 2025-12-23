namespace ProjectManagement.Application.DTOs.Projects
{
    public class CreateProjectDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "PLANNING";
        public string Priority { get; set; } = "MEDIUM";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string> TeamMemberEmails { get; set; } = new();
        public string? TeamLeadEmail { get; set; }
    }

}
