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
        // In ProjectManagement.Application/Services/ProjectService.cs

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto)
        {
            // 1. Map basic properties
            var project = _mapper.Map<Project>(dto);
            project.Id = Guid.NewGuid();

            // 2. Assign team lead
            if (!string.IsNullOrEmpty(dto.TeamLeadEmail))
            {
                var lead = await _userRepository.GetByEmailAsync(dto.TeamLeadEmail)
                           ?? throw new NotFoundException("Team lead not found");

                project.LeaderId = lead.Id;
                project.Leader = lead;
            }
            else
            {
                // PROPOSED FIX: Ensure Leader is required
                throw new ValidationException("Project Leader is required.");
            }

            // 3. Assign team members (avoid duplicates)
            if (dto.TeamMemberEmails != null)
            {
                foreach (var email in dto.TeamMemberEmails.Distinct())
                {
                    var user = await _userRepository.GetByEmailAsync(email)
                               ?? throw new NotFoundException($"User {email} not found");

                    project.ProjectMembers.Add(new ProjectMember
                    {
                        ProjectId = project.Id, // PROPOSED FIX: Explicitly set ProjectId
                        UserId = user.Id
                    });
                }
            }

            // 4. Save to DB
            await _projectRepository.AddAsync(project);

            // 5. Reload project including relations for accurate DTO
            var fullProject = await _projectRepository.GetByIdWithRelationsAsync(project.Id)
                              ?? throw new Exception("Failed to retrieve project after creation");

            // 6. Map to DTO
            return new ProjectDto
            {
                Id = fullProject.Id,
                Title = fullProject.Title,
                Description = fullProject.Description,
                Status = fullProject.Status,
                Priority = fullProject.Priority,
                StartDate = fullProject.StartDate,
                EndDate = fullProject.EndDate,
                LeaderId = fullProject.LeaderId,
                MemberIds = fullProject.ProjectMembers.Select(pm => pm.UserId).ToList()
            };
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
                Description = project.Description,
                Status = project.Status,
                Priority = project.Priority,               // new
                StartDate = project.StartDate,             // new
                EndDate = project.EndDate,                 // new
                TotalTasks = project.ProjectTasks.Count,
                CompletedTasks = project.ProjectTasks.Count(t => t.Status == "COMPLETED"),
                InProgressTasks = project.ProjectTasks.Count(t => t.Status != "COMPLETED"),
                TotalTeamMembers = project.ProjectMembers.Count,
                Progress = project.Progress,               // new
                TeamMemberEmails = project.ProjectMembers
                        .Where(pm => pm.User != null)
                        .Select(pm => pm.User.Email)
                        .ToList(),
                // new
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
        // Update project with nullable DTO fields
public async Task UpdateProjectAsync(Guid id, UpdateProjectDto dto)
{
    // Fetch project with members
    var project = await _projectRepository.GetByIdAsync(id, includeProjectMembers: true);
    if (project == null)
        throw new NotFoundException("Project not found");

    // --- Update basic fields ---
    if (dto.Title != null) project.Title = dto.Title;
    if (dto.Description != null) project.Description = dto.Description;
    if (dto.Status != null) project.Status = dto.Status;
    if (dto.Priority != null) project.Priority = dto.Priority;
    if (dto.StartDate.HasValue) project.StartDate = dto.StartDate;
    if (dto.EndDate.HasValue) project.EndDate = dto.EndDate;
    project.Progress = dto.Progress;

    // --- Update archive status only if provided ---
    if (dto.IsArchived.HasValue)
    {
        project.IsArchived = dto.IsArchived.Value;
    }

    // --- Update team lead only if provided ---
    if (dto.TeamLeadId.HasValue)
    {
        var lead = await _userRepository.GetByIdAsync(dto.TeamLeadId.Value);
        if (lead == null)
            throw new NotFoundException("Team lead not found");

        project.Leader = lead;
        project.LeaderId = lead.Id;
    }

    // --- Add new team members only ---
    if (dto.TeamMemberIds != null)
    {
        var existingMemberIds = project.ProjectMembers.Select(pm => pm.UserId).ToHashSet();

        foreach (var memberId in dto.TeamMemberIds)
        {
            if (!existingMemberIds.Contains(memberId))
            {
                var user = await _userRepository.GetByIdAsync(memberId);
                if (user != null)
                {
                    project.ProjectMembers.Add(new ProjectMember
                    {
                        ProjectId = project.Id,
                        UserId = user.Id
                    });
                }
            }
        }
    }

    // Save changes to DB
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

        public async Task<ProjectDto> ArchiveProjectAsync(Guid id)
{
    var project = await _projectRepository.GetByIdAsync(id);
    if (project == null)
        throw new NotFoundException("Project not found");

    project.IsArchived = true;
    await _projectRepository.UpdateAsync(project);

    return _mapper.Map<ProjectDto>(project);
}


       public async Task<ProjectDto> UnarchiveProjectAsync(Guid id)
{
    var project = await _projectRepository.GetByIdAsync(id);
    if (project == null)
        throw new NotFoundException("Project not found");

    project.IsArchived = false;
    await _projectRepository.UpdateAsync(project);

    return _mapper.Map<ProjectDto>(project);
}


    }
}
