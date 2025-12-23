using ProjectManagement.Application.DTOs.Dashboard;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetUserDashboardStatsAsync(Guid userId);
        Task<DashboardStatsDto> GetAdminDashboardStatsAsync();
    }
}
