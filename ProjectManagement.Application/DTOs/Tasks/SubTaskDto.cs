namespace ProjectManagement.Application.DTOs.Tasks
{
    public class SubTaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public Guid TaskId { get; set; }
    }
}
