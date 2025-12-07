namespace ProjectManagement.Application.DTOs.Projects
{
    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "PLANNING"; // default
        public string Priority { get; set; } = "MEDIUM";  // default
        public DateTime? StartDate { get; set; }           // optional
        public DateTime? EndDate { get; set; }             // optional
        public List<Guid> TeamMembers { get; set; } = new(); // user IDs
        public Guid? TeamLead { get; set; }               // user ID
    }
}
