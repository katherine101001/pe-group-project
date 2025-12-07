namespace ProjectManagement.Application.DTOs.Tasks
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public string Status { get; set; } = null!;
        public DateTime DueDate { get; set; }
    }
}
