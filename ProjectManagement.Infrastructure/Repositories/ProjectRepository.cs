using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Domain.Entities.Projects;
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Project project)
        {
            await _context.Project.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Project.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Project>> GetAllAsync()
        {
            return await _context.Project.ToListAsync();
        }

        // public async Task<Project?> GetByIdAsync(Guid id)
        // {
        //     return await _context.Project.FindAsync(id);
        // }

        public async Task<Project?> GetByIdAsync(Guid id, bool includeTasks = false, bool includeProjectMembers = false, bool includeLeader = false)
        {
            IQueryable<Project> query = _context.Project;

            if (includeTasks)
                query = query.Include(p => p.ProjectTasks);

            if (includeProjectMembers)
                query = query.Include(p => p.ProjectMembers);

            if (includeLeader)
                query = query.Include(p => p.Leader);

            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task<int> CountTeamMembersAsync(Guid projectId)
        {
            return await _context.Project
                .Where(p => p.Id == projectId)
                .Select(p => p.ProjectMembers.Count)
                .FirstOrDefaultAsync();
        }

    }
}
