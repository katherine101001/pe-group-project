namespace ProjectManagement.Application.DTOs.Dashboard
{
    public class DashboardProjectStatsDto
    {
        public int TotalProjects { get; set; }
        public int CompletedProjects { get; set; }
        public int MyTasks { get; set; }
        public int OverdueTasks { get; set; }
    }
}