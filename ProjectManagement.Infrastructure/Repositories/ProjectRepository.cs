// using Microsoft.EntityFrameworkCore;
// using ProjectManagement.Domain.Interfaces.Repositories;
// using ProjectManagement.Domain.Entities.Projects;
// using ProjectManagement.Infrastructure.Data;

// namespace ProjectManagement.Infrastructure.Repositories
// {
//     public class ProjectRepository : IProjectRepository
//     {
//         private readonly AppDbContext _context;

//         public ProjectRepository(AppDbContext context)
//         {
//             _context = context;
//         }

//         public async Task AddAsync(Project project)
//         {
//             await _context.Project.AddAsync(project);
//             await _context.SaveChangesAsync();
//         }

//         public async Task UpdateAsync(Project project)
//         {
//             _context.Project.Update(project);
//             await _context.SaveChangesAsync();
//         }

//         public async Task DeleteAsync(Project project)
//         {
//             _context.Project.Remove(project);
//             await _context.SaveChangesAsync();
//         }

//         public async Task<List<Project>> GetAllAsync()
//         {
//             return await _context.Project.ToListAsync();
//         }

//         public async Task<Project?> GetByIdAsync(Guid id, bool includeTasks = false, bool includeProjectMembers = false, bool includeLeader = false)
//         {
//             IQueryable<Project> query = _context.Project;

//             if (includeTasks)
//                 query = query.Include(p => p.ProjectTasks);

//             if (includeProjectMembers)
//                 query = query.Include(p => p.ProjectMembers);

//             if (includeLeader)
//                 query = query.Include(p => p.Leader);

//             return await query.FirstOrDefaultAsync(p => p.Id == id);
//         }


//         public async Task<int> CountTeamMembersAsync(Guid projectId)
//         {
//             return await _context.Project
//                 .Where(p => p.Id == projectId)
//                 .Select(p => p.ProjectMembers.Count)
//                 .FirstOrDefaultAsync();
//         }

//         public async Task<int> GetTotalProjectsAsync()
//         {
//             return await _context.Project.CountAsync(p => p.Status == "Active"); // 只统计 Active 项目
//         }

//         public async Task<List<Project>> SearchAsync(string keyword)
//         {
//             return await _context.Project
//                  .Where(p => p.Title.Contains(keyword) || (p.Description != null && p.Description.Contains(keyword)))
//                  .ToListAsync();

//         }

//         public async Task<int> GetCompletedProjectsAsync()
//         {
//             return await _context.Project
//                 .Where(p => p.Status == "COMPLETED")
//                 .CountAsync();
//         }

//         public async Task<int> GetActiveProjectsAsync()
//         {
//             return await _context.Project
//                 .Where(p => p.Status != "COMPLETED" && p.Status != "CANCELLED")
//                 .CountAsync();
//         }

//         public async Task<List<Project>> GetProjectsByUserAsync(Guid userId)
//         {
//             return await _context.Project
//                 .Where(p => p.LeaderId == userId)
//                 .Include(p => p.ProjectTasks)
//                 .ThenInclude(t => t.AssignToUser)
//                 .ToListAsync();
//         }

//         public async Task<int> CountOverdueTasksByProjectAsync(Guid projectId)
//         {
//             var today = DateTime.UtcNow;
//             return await _context.ProjectTask
//                 .Where(t => t.ProjectId == projectId && t.DueDate < today && t.Status != "COMPLETED")
//                 .CountAsync();
//         }



//     }
// }

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
            return await _context.Project.ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(Guid id, bool includeTasks = false, bool includeProjectMembers = false, bool includeLeader = false)
        {
            IQueryable<Project> query = _context.Project;

            if (includeTasks)
                query = query.Include(p => p.ProjectTasks).ThenInclude(t => t.AssignToUser);

            if (includeProjectMembers)
                query = query.Include(p => p.ProjectMembers);

            if (includeLeader)
                query = query.Include(p => p.Leader);

            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }

        // ---------- Dashboard / Stats Methods ----------
        public async Task<int> GetTotalProjectsAsync()
        {
            return await _context.Project.CountAsync(p => p.Status == "Active");
        }

        public async Task<int> GetCompletedProjectsAsync()
        {
            return await _context.Project.CountAsync(p => p.Status == "COMPLETED");
        }

        public async Task<int> GetActiveProjectsAsync()
        {
            return await _context.Project
                .Where(p => p.Status != "COMPLETED" && p.Status != "CANCELLED")
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



    }
}
