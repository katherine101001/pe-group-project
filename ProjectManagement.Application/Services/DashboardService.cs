using AutoMapper;
using ProjectManagement.Application.DTOs.Dashboard;
using ProjectManagement.Application.Interfaces.Repositories;
using ProjectManagement.Application.Interfaces.Services;

namespace ProjectManagement.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectTaskRepository _projectTaskRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public DashboardService(
            IProjectRepository projectRepository,
            IProjectTaskRepository projectTaskRepository,
            ICommentRepository commentRepository,
            INotificationRepository notificationRepository,
            IMapper mapper)
        {
            _projectRepository = projectRepository;
            _projectTaskRepository = projectTaskRepository;
            _commentRepository = commentRepository;
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            // Example: gather statistics
            var totalProjects = (await _projectRepository.GetAllAsync()).Count;
            var totalTasks = (await _projectTaskRepository.GetAllAsync()).Count;
            var totalComments = (await _commentRepository.GetAllAsync()).Count;
            var totalNotifications = (await _notificationRepository.GetAllAsync()).Count;

            // Create DTO
            var dashboardStats = new DashboardStatsDto
            {
                TotalProjects = totalProjects,
                TotalTasks = totalTasks,
                // TotalComments = totalComments,
                // TotalNotifications = totalNotifications
            };

            return dashboardStats;
        }
    }
}
