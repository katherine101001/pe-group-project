namespace ProjectManagement.Application.DTOs.Tasks
{
    public class MyTaskSidebarDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; } = null!;
        public string? Status { get; set; }
    }

}