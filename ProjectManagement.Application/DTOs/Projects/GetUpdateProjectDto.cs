
using System.Data;

namespace ProjectManagement.Application.DTOs.Projects
{
    public class GetUpdateProjectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Guid? TeamLeadId { get; set; }
        public List<Guid> TeamMemberIds { get; set; } = new List<Guid>();
    }
}