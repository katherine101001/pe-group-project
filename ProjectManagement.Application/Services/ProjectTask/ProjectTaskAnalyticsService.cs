using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Application.Interfaces.Services;


namespace ProjectManagement.Application.Services
{
    public class ProjectTaskAnalyticsService : IProjectTaskAnalyticsService
    {
        private readonly IProjectTaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectTaskAnalyticsService(
            IProjectTaskRepository taskRepository,
            IProjectRepository projectRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
        }

        public async Task<ProjectTaskAnalyticsDto> GetProjectAnalyticsAsync(Guid projectId)
        {
            var dto = new ProjectTaskAnalyticsDto
            {
                TotalTasks = await _taskRepository.CountTasksAsync(projectId),
                CompletedTasks = await _taskRepository.CountTasksByStatusAsync(projectId, "DONE"),
                ActiveTasks = await _taskRepository.CountTasksByStatusesAsync(projectId, new[] { "TO_DO", "IN_PROGRESS" }),
                OverdueTasks = await _taskRepository.CountOverdueTasksAsync(projectId),
                TeamSize = await _projectRepository.CountTeamMembersAsync(projectId),

                TasksByStatus = await _taskRepository.GetTaskCountsGroupedByStatusAsync(projectId),
                TasksByType = await _taskRepository.GetTaskCountsGroupedByTypeAsync(projectId),
                TasksByPriority = await _taskRepository.GetTaskCountsGroupedByPriorityAsync(projectId)
            };

            return dto;
        }
    }
}
