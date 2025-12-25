using System.Data;

namespace ProjectManagement.Application.DTOs.Projects
{
    // In the Project Page (when click the "Project" button at side bar)
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "PLANNING";
        public string Priority { get; set; } = "MEDIUM";
        public int Progress { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid LeaderId { get; set; }
        public List<Guid> MemberIds { get; set; } = new();

        public bool IsArchived { get; set; } = false;
    }
}
