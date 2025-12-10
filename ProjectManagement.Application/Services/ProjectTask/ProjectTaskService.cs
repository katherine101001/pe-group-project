using AutoMapper;
using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Application.Interfaces.Services;
using ProjectManagement.Domain.Entities.ProjectTasks;
using ProjectManagement.Shared.Exceptions;

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
            if (dto.AssignTo.HasValue)
            {
                var userExists = await _userRepository.GetByIdAsync(dto.AssignTo.Value);
                if (userExists == null)
                {
                    throw new NotFoundException("Assignee user not found.");
                }
            }

            // Map DTO -> Entity
            var task = _mapper.Map<ProjectTask>(dto);

            // Save entity
            await _projectTaskRepository.AddAsync(task);

            var createdTask = await _projectTaskRepository.GetByIdAsync(task.Id);
            return _mapper.Map<ProjectTaskDto>(createdTask!);
        }

        public async Task<ProjectTaskDto?> GetProjectTaskByIdAsync(Guid id)
        {
            var task = await _projectTaskRepository.GetByIdAsync(id);

            if (task == null)
                return null;

            return _mapper.Map<ProjectTaskDto>(task);
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

        public async Task<ProjectTaskDetails?> GetProjectTaskBrieflyByIdAsync(Guid id)
        {
            var task = await _projectTaskRepository.GetByIdAsync(id);

            if (task == null)
                return null;

            return _mapper.Map<ProjectTaskDetails>(task);
        }

        public async Task<List<ProjectTaskDetails>> GetAllProjectTasksBrieflyAsync()
        {
            var tasks = await _projectTaskRepository.GetAllAsync();
            return _mapper.Map<List<ProjectTaskDetails>>(tasks);
        }
    }
}
