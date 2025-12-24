using AutoMapper;
using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Application.Interfaces.Services;
using ProjectManagement.Domain.Entities.ProjectTasks;
using ProjectManagement.Shared.Exceptions;
using ProjectManagement.Application.DTOs;

namespace ProjectManagement.Application.Services
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IProjectTaskRepository _projectTaskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ProjectTaskService(IProjectTaskRepository projectTaskRepository, IUserRepository userRepository, IMapper mapper)
        {
            _projectTaskRepository = projectTaskRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // Create Task for the Project
public async Task<ProjectTaskDto> CreateProjectTaskAsync(CreateProjectTaskDto dto)
    {
        // 允许可空 AssignToUserId
        if (dto.AssignToUserId != Guid.Empty)
        {
            var userExists = await _userRepository.GetByIdAsync(dto.AssignToUserId);
            if (userExists == null)
            {
                throw new NotFoundException("Assignee user not found.");
            }
        }

        // Map DTO -> Entity
        var task = _mapper.Map<ProjectTask>(dto);

        // 保存任务
        await _projectTaskRepository.AddAsync(task);

        // 取出任务，并 Include AssignToUser
        var createdTask = await _projectTaskRepository.GetByIdWithAssigneeAsync(task.Id);

        // 映射 DTO 返回前端
        return _mapper.Map<ProjectTaskDto>(createdTask!);
    }

        public async Task<ProjectTaskDto?> GetProjectTaskByIdAsync(Guid id)
        {
            var task = await _projectTaskRepository.GetByIdAsync(id);

            if (task == null)
                return null;

            return _mapper.Map<ProjectTaskDto>(task);
        }

        public async Task<ProjectTaskDto> GetUpdateProjectTaskByIdAsync(Guid id)
        {
            var task = await _projectTaskRepository.GetByIdAsync(id);

            if (task == null)
                throw new NotFoundException("Task not found");

            // Map entity to DTO for update
            var dto = _mapper.Map<ProjectTaskDto>(task);

            // Optionally include assignee info
            if (task.AssignToUser != null)
            {
                dto.AssignToUserId = task.AssignToUserId;
                dto.AssigneeName = task.AssignToUser.Name; // or map only needed fields
            }

            return dto;
        }

        public async Task<List<ProjectTaskDto>> GetTasksByProjectIdAsync(Guid projectId)
        {
            var tasks = await _projectTaskRepository.GetByProjectIdAsync(projectId);
            return _mapper.Map<List<ProjectTaskDto>>(tasks);
        }


        public async Task<List<ProjectTaskDto>> GetAllProjectTasksAsync()
        {
            var tasks = await _projectTaskRepository.GetAllAsync();
            return _mapper.Map<List<ProjectTaskDto>>(tasks);
        }

        public async Task UpdateProjectTaskAsync(Guid id, ProjectTaskDto dto)
        {
            var existingTask = await _projectTaskRepository.GetByIdAsync(id);

            if (existingTask == null)
                throw new NotFoundException("Task not found");

            // Map updated values from DTO to entity
            _mapper.Map(dto, existingTask);

            await _projectTaskRepository.UpdateAsync(existingTask);
        }

        public async Task DeleteProjectTaskAsync(Guid id)
        {
            var existingTask = await _projectTaskRepository.GetByIdAsync(id);

            if (existingTask == null)
                throw new NotFoundException("Task not found");

            await _projectTaskRepository.DeleteAsync(existingTask);
        }

        public async Task<ProjectTaskDetailsDto?> GetProjectTaskBrieflyByIdAsync(Guid id)
        {
            var task = await _projectTaskRepository.GetByIdAsync(id);

            if (task == null)
                return null;

            return _mapper.Map<ProjectTaskDetailsDto>(task);
        }

        public async Task<List<ProjectTaskDetailsDto>> GetAllProjectTasksBrieflyAsync()
        {
            var tasks = await _projectTaskRepository.GetAllAsync();
            return _mapper.Map<List<ProjectTaskDetailsDto>>(tasks);
        }

        public async Task<List<SearchTaskDto>> SearchTasksAsync(string keyword)
        {
            var tasks = await _projectTaskRepository.SearchAsync(keyword);
            return _mapper.Map<List<SearchTaskDto>>(tasks);

        }


       public async Task<List<ProjectTaskCalendarDto>> GetTaskCalendarAsync(int year, int month, Guid? userId = null, string? role = null)
        {
            List<ProjectTask> tasks;

            if (role == "LEADER" || userId == null)
            {
                // Leader 可以看全部任务
                tasks = await _projectTaskRepository.GetTasksByMonthAsync(year, month);
            }
            else
            {
                // Member 只看自己的任务
                tasks = await _projectTaskRepository.GetTasksByMonthForUserAsync(year, month, userId.Value);
            }

            // 按日期分组
            var calendarDtos = tasks
                .GroupBy(t => t.DueDate?.Date)
                .Select(g => new ProjectTaskCalendarDto
                {
                    Date = g.Key?.ToString("yyyy-MM-dd"),
                    TaskCount = g.Count(),
                    Tasks = g.Select(t => new ProjectTaskDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Type = t.Type,
                        Status = t.Status,
                        Priority = t.Priority,
                        AssignToUserId = t.AssignToUserId,
                        AssigneeName = t.AssignToUser != null ? t.AssignToUser.Name : null,
                        DueDate = t.DueDate
                    }).ToList()
                })
                .ToList();

            return calendarDtos;
        }

        public async Task<List<OverdueTaskDto>> GetAllOverdueTasksAsync()
        {
            var tasks = await _projectTaskRepository.GetAllOverdueTasksAsync();
            return _mapper.Map<List<OverdueTaskDto>>(tasks);
        }

        public async Task<List<OverdueTaskDto>> GetOverdueTasksByProjectIdAsync(Guid projectId)
        {
            var tasks = await _projectTaskRepository.GetOverdueTasksByProjectIdAsync(projectId);
            return _mapper.Map<List<OverdueTaskDto>>(tasks);
        }

        public async Task<int> GetSoonToOverdueTaskCountAsync()
        {
            return await _projectTaskRepository.CountSoonToOverdueTasksAsync();
        }




        public async Task<List<RecentActivityDto>> GetRecentTasksAsync(int limit)
        {
            var tasks = await _projectTaskRepository.GetRecentTasksAsync(limit);

            return _mapper.Map<List<RecentActivityDto>>(tasks);
        }

        public async Task<List<MyTaskSidebarDto>> GetTasksByUserAsync(Guid userId)
        {
            var tasks = await _projectTaskRepository.GetTasksByUserAsync(userId);
            return _mapper.Map<List<MyTaskSidebarDto>>(tasks);
        }

        public async Task UpdateTaskStatusAsync(Guid taskId, string newStatus)
        {
            var task = await _projectTaskRepository.GetByIdAsync(taskId);

            if (task == null)
                throw new NotFoundException("Task not found");

            task.Status = newStatus;
            task.UpdatedAt = DateTime.UtcNow;

            await _projectTaskRepository.UpdateAsync(task);
        }



    }

}
    
