using AutoMapper;
using ProjectManagement.Application.DTOs.Projects;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Application.Interfaces.Services;
using ProjectManagement.Domain.Entities.Projects;
using ProjectManagement.Shared.Exceptions;

namespace ProjectManagement.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateProjectAsync(CreateProjectDto dto)
        {
            var project = _mapper.Map<Project>(dto);

            // Assign team lead
            if (dto.TeamLeadId.HasValue)
            {
                var lead = await _userRepository.GetByIdAsync(dto.TeamLeadId.Value);
                if (lead == null)
                    throw new NotFoundException("Team lead not found");
                project.Leader = lead;
            }

            //Assign team members
            foreach (var memberId in dto.TeamMemberIds)
            {
                var user = await _userRepository.GetByIdAsync(memberId);
                if (user != null)
                {
                    project.ProjectMembers.Add(new ProjectMember { Project = project, User = user });
                }
                else
                {
                    throw new NotFoundException($"User with ID {memberId} not found");
                }
            }

            foreach (var memberId in dto.TeamMemberIds) // Guid list from dropdown
            {
                var user = await _userRepository.GetByIdAsync(memberId);
                if (user != null)
                {
                    project.ProjectMembers.Add(new ProjectMember { Project = project, User = user });
                }
                else
                {
                    throw new NotFoundException($"User with ID {memberId} not found");
                }
            }

            // Save project
            await _projectRepository.AddAsync(project);
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
