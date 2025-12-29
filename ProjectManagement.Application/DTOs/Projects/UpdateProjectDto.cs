namespace ProjectManagement.Application.DTOs.Projects
{
    public class UpdateProjectDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; } // default
        public string? Priority { get; set; }  // default
        public int Progress { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<Guid> TeamMemberIds { get; set; } = new(); // user IDs
        public Guid? TeamLeadId { get; set; }               // user ID
        public bool? IsArchived { get; set; }

    }

}
