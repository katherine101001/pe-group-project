using ProjectManagement.Application.DTOs.Tasks;
namespace ProjectManagement.Application.DTOs.Dashboard
{
    public class DashboardTasksDto
    {
        public int MyTasksCount { get; set; }
        public int InProgressTasksCount { get; set; }
        public int OverdueTasksCount { get; set; }

        public int SoonToOverdueTasksCount { get; set; }
        public List<OverdueTaskDto> SoonToOverdueTasks { get; set; } = new();

        public List<MyTaskSidebarDto> MyTasks { get; set; } = new();
        public List<OverdueTaskDto> OverdueTasks { get; set; } = new();
    }
}
