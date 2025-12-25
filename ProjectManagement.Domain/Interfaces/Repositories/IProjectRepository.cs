using ProjectManagement.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(Guid id, bool includeTasks = false, bool includeProjectMembers = false, bool includeLeader = false);
        Task<List<Project>> GetAllAsync();

        Task<int> CountTeamMembersAsync(Guid projectId);


        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
        Task<int> GetTotalProjectsAsync();

        Task<List<Project>> SearchAsync(string keyword);
        Task<int> GetActiveProjectsAsync();
        Task<int> GetCompletedProjectsAsync();

        Task<int> GetTotalProjectsByUserAsync(Guid userId);
        Task<int> GetCompletedProjectsByUserAsync(Guid userId);
        Task<int> GetActiveProjectsByUserAsync(Guid userId);
        Task<Project?> GetByIdWithRelationsAsync(Guid id);
        Task ArchiveAsync(Guid projectId);
        Task RestoreAsync(Guid projectId);

    }
}
