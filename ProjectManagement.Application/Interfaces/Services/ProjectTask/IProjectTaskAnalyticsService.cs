using ProjectManagement.Application.DTOs.Tasks;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface IProjectTaskAnalyticsService
    {
        Task<ProjectTaskAnalyticsDto> GetProjectAnalyticsAsync(Guid projectId);
    }
}
