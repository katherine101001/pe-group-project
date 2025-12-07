using AutoMapper;
using ProjectManagement.Application.DTOs.Projects;
using ProjectManagement.Application.Interfaces.Repositories;
using ProjectManagement.Application.Interfaces.Services;
using ProjectManagement.Domain.Entities.Projects;
using ProjectManagement.Shared.Exceptions;

namespace ProjectManagement.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ProjectDto> CreateProjectAsync(ProjectDto dto)
        {
            // Map DTO -> Entity
            var project = _mapper.Map<Project>(dto);

            // Save entity
            await _projectRepository.AddAsync(project);

            // Map Entity -> DTO
            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);

            if (project == null)
                return null;

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<List<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return _mapper.Map<List<ProjectDto>>(projects);
        }

        public async Task UpdateProjectAsync(Guid id, ProjectDto dto)
        {
            var existingProject = await _projectRepository.GetByIdAsync(id);

            if (existingProject == null)
                throw new NotFoundException("Project not found");

            // Map updated values from DTO to entity
            _mapper.Map(dto, existingProject);

            await _projectRepository.UpdateAsync(existingProject);
        }

        public async Task DeleteProjectAsync(Guid id)
        {
            var existingProject = await _projectRepository.GetByIdAsync(id);

            if (existingProject == null)
                throw new NotFoundException("Project not found");

            await _projectRepository.DeleteAsync(existingProject);
        }
    }
}
