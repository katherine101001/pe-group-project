using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities.Projects;
using ProjectManagement.Domain.Interfaces.Repositories;
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

        // ---------- CRUD ----------
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
            return await _context.Project
                .Include(p => p.ProjectMembers)
                .ThenInclude(pm => pm.User) // include the join table
                .ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(Guid id, bool includeTasks = false, bool includeProjectMembers = false, bool includeLeader = false)
        {
            IQueryable<Project> query = _context.Project;

            if (includeTasks)
                query = query.Include(p => p.ProjectTasks).ThenInclude(t => t.AssignToUser);

            if (includeProjectMembers)
                query = query.Include(p => p.ProjectMembers)
                        .ThenInclude(pm => pm.User);

            if (includeLeader)
                query = query.Include(p => p.Leader);

            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }

        // Admin stats
        public async Task<int> GetTotalProjectsAsync()
        {
            return await _context.Project
                .Where(p => !p.IsArchived) // exclude archived
                .CountAsync();
        }

        public async Task<int> GetCompletedProjectsAsync()
        {
            return await _context.Project
                .Where(p => !p.IsArchived && p.Progress == 100) // completed if progress is 100%
                .CountAsync();
        }


        public async Task<int> GetActiveProjectsAsync()
        {
            return await _context.Project
                .Where(p => !p.IsArchived && p.Status != "COMPLETED" && p.Status != "CANCELLED") // exclude archived
                .CountAsync();
        }

        // User-specific stats
        public async Task<int> GetTotalProjectsByUserAsync(Guid userId)
        {
            return await _context.Project
                .Where(p => !p.IsArchived && (p.LeaderId == userId || p.ProjectMembers.Any(m => m.UserId == userId)))
                .CountAsync();
        }

        public async Task<int> GetCompletedProjectsByUserAsync(Guid userId)
        {
            return await _context.Project
                .Where(p => !p.IsArchived
                            && p.Progress == 100 // completed if progress is 100%
                            && (p.LeaderId == userId || p.ProjectMembers.Any(m => m.UserId == userId)))
                .CountAsync();
        }


        public async Task<int> GetActiveProjectsByUserAsync(Guid userId)
        {
            return await _context.Project
                .Where(p => !p.IsArchived && (p.LeaderId == userId || p.ProjectMembers.Any(m => m.UserId == userId))
                            && p.Status != "COMPLETED" && p.Status != "CANCELLED")
                .CountAsync();
        }

        public async Task<int> CountTeamMembersAsync(Guid projectId)
        {
            return await _context.Project
                .Where(p => p.Id == projectId)
                .Select(p => p.ProjectMembers.Count)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Project>> GetProjectsByUserAsync(Guid userId)
        {
            return await _context.Project
                .Where(p => p.LeaderId == userId)
                .Include(p => p.ProjectTasks)
                .ThenInclude(t => t.AssignToUser)
                .ToListAsync();
        }

        public async Task<int> CountOverdueTasksByProjectAsync(Guid projectId)
        {
            var today = DateTime.UtcNow;
            return await _context.ProjectTask
                .Where(t => t.ProjectId == projectId && t.DueDate < today && t.Status != "COMPLETED")
                .CountAsync();
        }

        public async Task<List<Project>> SearchAsync(string keyword)
        {
            return await _context.Project
                .Where(p => p.Title.Contains(keyword) || (p.Description != null && p.Description.Contains(keyword)))
                .ToListAsync();
        }

        public async Task<Project?> GetByIdWithRelationsAsync(Guid id)
        {
            return await _context.Project
                .Include(p => p.Leader)
                .Include(p => p.ProjectMembers)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task ArchiveAsync(Guid projectId)
        {
            var project = await GetByIdAsync(projectId);
            if (project == null) return;

            project.IsArchived = true;
            _context.Project.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task RestoreAsync(Guid projectId)
        {
            var project = await GetByIdAsync(projectId);
            if (project == null) return;

            project.IsArchived = false;
            _context.Project.Update(project);
            await _context.SaveChangesAsync();
        }


    }
}
