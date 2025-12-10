using ProjectManagement.Domain.Entities.Projects;

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
    }
}
