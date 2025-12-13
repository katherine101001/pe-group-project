using ProjectManagement.Application.DTOs.Dashboard;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();

        Task<DashboardProjectStatsNumDto> GetDashboardProjectStatsAsync(Guid userId);

    }
}
