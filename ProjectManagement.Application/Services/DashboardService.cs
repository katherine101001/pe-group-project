using AutoMapper;
using ProjectManagement.Application.DTOs.Dashboard;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Application.Interfaces.Services;

// // namespace ProjectManagement.Application.Services
// // {
// //     public class DashboardService : IDashboardService
// //     {
// //         private readonly IProjectRepository _projectRepository;
// //         private readonly IProjectTaskRepository _projectTaskRepository;
// //         private readonly ICommentRepository _commentRepository;
// //         private readonly INotificationRepository _notificationRepository;
// //         private readonly IMapper _mapper;

// //         public DashboardService(
// //             IProjectRepository projectRepository,
// //             IProjectTaskRepository projectTaskRepository,
// //             ICommentRepository commentRepository,
// //             INotificationRepository notificationRepository,
// //             IMapper mapper)
// //         {
// //             _projectRepository = projectRepository;
// //             _projectTaskRepository = projectTaskRepository;
// //             _commentRepository = commentRepository;
// //             _notificationRepository = notificationRepository;
// //             _mapper = mapper;
// //         }

// //         public async Task<DashboardStatsDto> GetDashboardStatsAsync()
// //         {
// //             // Example: gather statistics
// //             var totalProjects = (await _projectRepository.GetAllAsync()).Count;
// //             var totalTasks = (await _projectTaskRepository.GetAllAsync()).Count;
// //             var totalComments = (await _commentRepository.GetAllAsync()).Count;
// //             var totalNotifications = (await _notificationRepository.GetAllAsync()).Count;

// //             // Create DTO
// //             var dashboardStats = new DashboardStatsDto
// //             {
// //                 TotalProjects = totalProjects,
// //                 TotalTasks = totalTasks,
// //                 // TotalComments = totalComments,
// //                 // TotalNotifications = totalNotifications
// //             };

// //             return dashboardStats;
// //         }

// //         public async Task<DashboardProjectStatsNumDto> GetDashboardProjectStatsAsync(Guid userId)
// //         {
// //             return new DashboardProjectStatsNumDto
// //             {
// //                 TotalProjects = await _projectRepository.GetTotalProjectsAsync(),
// //                 CompletedProjects = await _projectRepository.GetCompletedProjectsAsync(),
// //                 MyTasks = await _projectTaskRepository.GetMyTasksCountAsync(userId),
// //                 //OverdueTasks = await _projectTaskRepository.GetOverdueTasksCountAsync(userId)
// //             };
// //         }

// //     }
// // }

// using ProjectManagement.Application.DTOs.Dashboard;
// using ProjectManagement.Application.Interfaces.Services;
// using ProjectManagement.Domain.Interfaces.Repositories;

namespace ProjectManagement.Application.Services
{
    // public class DashboardService : IDashboardService
    // {
    //     private readonly IProjectRepository _projectRepository;
    //     private readonly IProjectTaskRepository _projectTaskRepository;
    //     private readonly IUserRepository _userRepository;

    //     public DashboardService(
    //         IProjectRepository projectRepository,
    //         IProjectTaskRepository projectTaskRepository,
    //         IUserRepository userRepository)
    //     {
    //         _projectRepository = projectRepository;
    //         _projectTaskRepository = projectTaskRepository;
    //         _userRepository = userRepository;
    //     }

    //     public async Task<DashboardStatsDto> GetDashboardStatsAsync(Guid userId)
    //     {
    //         // Projects
    //         var totalProjects = await _projectRepository.GetTotalProjectsAsync();
    //         var completedProjects = await _projectRepository.GetCompletedProjectsAsync();
    //         var activeProjects = await _projectRepository.GetActiveProjectsAsync();

    //         // Tasks
    //         var myTasks = await _projectTaskRepository.GetMyTasksCountAsync(userId);
    //         var overdueTasks = await _projectTaskRepository.GetOverdueTasksCountAsync(userId);

    //         return new DashboardStatsDto
    //         {
    //             TotalProjects = totalProjects,
    //             CompletedProjects = completedProjects,
    //             ActiveProjects = activeProjects,
    //             MyTasks = myTasks,
    //             OverdueTasks = overdueTasks
    //         };
    //     }
    // }
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
}

