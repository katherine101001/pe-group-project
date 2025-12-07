namespace ProjectManagement.Application.DTOs.Projects
{
    public class ProjectOverviewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
    }
}
