namespace ProjectManagement.Application.DTOs.Tasks
{
    public class ProjectTaskCalendarDto
    {
       public string? Date { get; set; }= null!;
        public int TaskCount { get; set; }

        public List<ProjectTaskDto> Tasks { get; set; } = new List<ProjectTaskDto>();
    }
}
