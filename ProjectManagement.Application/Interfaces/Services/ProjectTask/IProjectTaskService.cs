using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Application.DTOs.Projects;
using ProjectManagement.Application.DTOs;

namespace ProjectManagement.Application.Interfaces.Services
{
        public interface IProjectTaskService
        {
                Task<ProjectTaskDto> CreateProjectTaskAsync(CreateProjectTaskDto dto);
                Task<ProjectTaskDto?> GetProjectTaskByIdAsync(Guid id);
                Task<List<ProjectTaskDto>> GetAllProjectTasksAsync();
                Task UpdateProjectTaskAsync(Guid id, ProjectTaskDto dto);
                Task DeleteProjectTaskAsync(Guid id);


                Task<ProjectTaskDetailsDto?> GetProjectTaskBrieflyByIdAsync(Guid id);
                Task<List<ProjectTaskDetailsDto>> GetAllProjectTasksBrieflyAsync();

                Task<List<SearchTaskDto>> SearchTasksAsync(string keyword);
                Task<List<ProjectTaskCalendarDto>> GetTaskCalendarAsync(int year, int month);
                Task<List<OverdueTaskDto>> GetAllOverdueTasksAsync();

                Task<List<OverdueTaskDto>> GetOverdueTasksByProjectIdAsync(Guid projectId);
                Task<List<RecentActivityDto>> GetRecentTasksAsync(int limit);
                Task<int> GetSoonToOverdueTaskCountAsync();
                Task<List<MyTaskSidebarDto>> GetTasksByUserAsync(Guid userId);
                
        }
}
