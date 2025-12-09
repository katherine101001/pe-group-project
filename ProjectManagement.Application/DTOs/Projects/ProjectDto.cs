using System.Data;

namespace ProjectManagement.Application.DTOs.Projects
{
    // In the Project Page (when click the "Project" button at side bar)
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
