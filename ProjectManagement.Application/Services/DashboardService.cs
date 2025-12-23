using AutoMapper;
using ProjectManagement.Application.DTOs.Dashboard;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Application.Interfaces.Services;

public class DashboardService : IDashboardService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTaskRepository _projectTaskRepository;

    public DashboardService(
        IProjectRepository projectRepository,
        IProjectTaskRepository projectTaskRepository)
    {
        _projectRepository = projectRepository;
        _projectTaskRepository = projectTaskRepository;
    }

    // ADMIN dashboard (system-wide)
    public async Task<DashboardStatsDto> GetAdminDashboardStatsAsync()
    {
        return new DashboardStatsDto
        {
            TotalProjects = await _projectRepository.GetTotalProjectsAsync(),
            CompletedProjects = await _projectRepository.GetCompletedProjectsAsync(),
            ActiveProjects = await _projectRepository.GetActiveProjectsAsync(),
            MyTasks = 0, // admin does not have "my tasks"
            OverdueTasks = await _projectTaskRepository.GetOverdueTasksCountAsync()
        };
    }

    // USER dashboard (personalized: leader or member)
    public async Task<DashboardStatsDto> GetUserDashboardStatsAsync(Guid userId)
    {
        return new DashboardStatsDto
        {
            // Get only projects the user is part of
            TotalProjects = await _projectRepository.GetTotalProjectsByUserAsync(userId),
            CompletedProjects = await _projectRepository.GetCompletedProjectsByUserAsync(userId),
            ActiveProjects = await _projectRepository.GetActiveProjectsByUserAsync(userId),

            // Get only tasks assigned to the user
            MyTasks = await _projectTaskRepository.GetMyTasksCountAsync(userId),
            OverdueTasks = await _projectTaskRepository.GetOverdueTasksCountAsync(userId)
        };
    }

}


