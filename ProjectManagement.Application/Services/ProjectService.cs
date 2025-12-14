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

        // Create Project (Add New project)
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

        // When clicking into the project overview
        public async Task<ProjectOverviewDto?> GetProjectByIdAsync(Guid id)
        {
            // Fetch project including related tasks and team members
            var project = await _projectRepository.GetByIdAsync(id, includeTasks: true, includeProjectMembers: true);

            if (project == null)
                return null;

            // Map to ProjectOverviewDto manually (because we need calculated fields)
            var overview = new ProjectOverviewDto
            {
                Id = project.Id,
                Title = project.Title,
                Status = project.Status,
                TotalTasks = project.ProjectTasks.Count,
                CompletedTasks = project.ProjectTasks.Count(t => t.Status == "COMPLETED"),
                InProgressTasks = project.ProjectTasks.Count(t => t.Status == "IN_PROGRESS"),
                TotalTeamMembers = project.ProjectMembers.Count
            };

            return overview;
        }


        // To display ALL Projects when clicking at the project button at side bar
        public async Task<List<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return _mapper.Map<List<ProjectDto>>(projects);
        }


        // Fetch the project details to fill in the form for Project Setting Page
        public async Task<GetUpdateProjectDto> GetUpdateProjectByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id, includeProjectMembers: true, includeLeader: true);
            if (project == null)
                throw new NotFoundException("Project not found");

            var dto = _mapper.Map<GetUpdateProjectDto>(project);

            // Include team members
            dto.TeamMemberIds = project.ProjectMembers.Select(pm => pm.UserId).ToList();
            dto.TeamLeadId = project.Leader?.Id;

            return dto;
        }


        // Update project with nullable DTO fields
        public async Task UpdateProjectAsync(Guid id, UpdateProjectDto dto)
        {
            var project = await _projectRepository.GetByIdAsync(id, includeProjectMembers: true);
            if (project == null)
                throw new NotFoundException("Project not found");

            // Update fields only if provided
            if (dto.Title != null) project.Title = dto.Title;
            if (dto.Description != null) project.Description = dto.Description;
            if (dto.Status != null) project.Status = dto.Status;
            if (dto.Priority != null) project.Priority = dto.Priority;
            if (dto.StartDate.HasValue) project.StartDate = dto.StartDate;
            if (dto.EndDate.HasValue) project.EndDate = dto.EndDate;

            // Update team lead if provided
            if (dto.TeamLeadId.HasValue)
            {
                var lead = await _userRepository.GetByIdAsync(dto.TeamLeadId.Value);
                if (lead == null)
                    throw new NotFoundException("Team lead not found");
                project.Leader = lead;
            }

            // Update team members if provided
            if (dto.TeamMemberIds != null)
            {
                // Get current member IDs
                var currentMemberIds = project.ProjectMembers.Select(pm => pm.UserId).ToHashSet();

                foreach (var memberId in dto.TeamMemberIds)
                {
                    // Only add if not already present
                    if (!currentMemberIds.Contains(memberId))
                    {
                        var user = await _userRepository.GetByIdAsync(memberId);
                        if (user != null)
                        {
                            // Attach the existing user entity instead of creating a new one
                            project.ProjectMembers.Add(new ProjectMember
                            {
                                ProjectId = project.Id,
                                UserId = user.Id
                            });
                        }
                    }
                }
            }


            await _projectRepository.UpdateAsync(project);
        }


        public async Task DeleteProjectAsync(Guid id)
        {
            var existingProject = await _projectRepository.GetByIdAsync(id);

            if (existingProject == null)
                throw new NotFoundException("Project not found");

            await _projectRepository.DeleteAsync(existingProject);
        }

        public async Task<List<SearchProjectDto>> SearchProjectsAsync(string keyword)
        {
            var projects = await _projectRepository.SearchAsync(keyword);
            return _mapper.Map<List<SearchProjectDto>>(projects);
        }
        


    }
}
