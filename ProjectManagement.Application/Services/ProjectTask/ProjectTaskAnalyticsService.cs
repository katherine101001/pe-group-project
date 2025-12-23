// using ProjectManagement.Application.DTOs.Tasks;
// using ProjectManagement.Domain.Interfaces.Repositories;
// using ProjectManagement.Application.Interfaces.Services;


// namespace ProjectManagement.Application.Services
// {
//     public class ProjectTaskAnalyticsService : IProjectTaskAnalyticsService
//     {
//         private readonly IProjectTaskRepository _taskRepository;
//         private readonly IProjectRepository _projectRepository;

//         public ProjectTaskAnalyticsService(
//             IProjectTaskRepository taskRepository,
//             IProjectRepository projectRepository)
//         {
//             _taskRepository = taskRepository;
//             _projectRepository = projectRepository;
//         }

//         public async Task<ProjectTaskAnalyticsDto> GetProjectAnalyticsAsync(Guid projectId)
//         {
//             var dto = new ProjectTaskAnalyticsDto
//             {
//                 TotalTasks = await _taskRepository.CountTasksAsync(projectId),
//                 CompletedTasks = await _taskRepository.CountTasksByStatusAsync(projectId, "DONE"),
//                 ActiveTasks = await _taskRepository.CountTasksByStatusesAsync(projectId, new[] { "TO_DO", "IN_PROGRESS" }),
//                 OverdueTasks = await _taskRepository.CountOverdueTasksAsync(projectId),
//                 TeamSize = await _projectRepository.CountTeamMembersAsync(projectId),

//                 TasksByStatus = await _taskRepository.GetTaskCountsGroupedByStatusAsync(projectId),
//                 TasksByType = await _taskRepository.GetTaskCountsGroupedByTypeAsync(projectId),
//                 TasksByPriority = await _taskRepository.GetTaskCountsGroupedByPriorityAsync(projectId)
//             };

//             return dto;
//         }
//     }
// }

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
            // Run independent queries in parallel
            var totalTasksTask = _taskRepository.CountTasksAsync(projectId);
            var completedTasksTask = _taskRepository.CountTasksByStatusAsync(projectId, "DONE");
            var activeTasksTask = _taskRepository.CountTasksByStatusesAsync(projectId, new[] { "TO_DO", "IN_PROGRESS" });
            var overdueTasksTask = _taskRepository.CountOverdueTasksAsync(projectId);
            var teamSizeTask = _projectRepository.CountTeamMembersAsync(projectId);
            var tasksByStatusTask = _taskRepository.GetTaskCountsGroupedByStatusAsync(projectId);
            var tasksByTypeTask = _taskRepository.GetTaskCountsGroupedByTypeAsync(projectId);
            var tasksByPriorityTask = _taskRepository.GetTaskCountsGroupedByPriorityAsync(projectId);

            await Task.WhenAll(totalTasksTask, completedTasksTask, activeTasksTask, overdueTasksTask,
                               teamSizeTask, tasksByStatusTask, tasksByTypeTask, tasksByPriorityTask);

            return new ProjectTaskAnalyticsDto
            {
                TotalTasks = totalTasksTask.Result,
                CompletedTasks = completedTasksTask.Result,
                ActiveTasks = activeTasksTask.Result,
                OverdueTasks = overdueTasksTask.Result,
                TeamSize = teamSizeTask.Result,
                TasksByStatus = tasksByStatusTask.Result,
                TasksByType = tasksByTypeTask.Result,
                TasksByPriority = tasksByPriorityTask.Result
            };
        }
    }
}
