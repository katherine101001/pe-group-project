using AutoMapper;
using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Application.Interfaces.Repositories;
using ProjectManagement.Application.Interfaces.Services;
using ProjectManagement.Domain.Entities.Tasks;
using ProjectManagement.Shared.Exceptions;

namespace ProjectManagement.Application.Services
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IProjectTaskRepository _projectTaskRepository;
        private readonly IMapper _mapper;

        public ProjectTaskService(IProjectTaskRepository projectTaskRepository, IMapper mapper)
        {
            _projectTaskRepository = projectTaskRepository;
            _mapper = mapper;
        }

        public async Task<ProjectTaskDto> CreateProjectTaskAsync(CreateTaskDto dto)
        {
            // Map DTO -> Entity
            var task = _mapper.Map<ProjectTask>(dto);

            task.Id = Guide.NewGuid();
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;//update

            // Save entity
            await _projectTaskRepository.AddAsync(task);

            // Map Entity -> DTO
            return _mapper.Map<ProjectTaskDto>(task);
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
    }
}
