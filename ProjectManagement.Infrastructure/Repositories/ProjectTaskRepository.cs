using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Domain.Entities.ProjectTasks;
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class ProjectTaskRepository : IProjectTaskRepository
    {
        private readonly AppDbContext _context;

        public ProjectTaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProjectTask projectTask)
        {
            await _context.ProjectTask.AddAsync(projectTask);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProjectTask projectTask)
        {
            _context.ProjectTask.Update(projectTask);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProjectTask projectTask)
        {
            _context.ProjectTask.Remove(projectTask);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProjectTask>> GetAllAsync()
        {
            return await _context.ProjectTask
                                 .Include(t => t.AssignToUser)   // <-- 关键
                                 .ToListAsync();
        }

        public async Task<ProjectTask?> GetByIdAsync(Guid id)
        {
            return await _context.ProjectTask
                                 .Include(t => t.AssignToUser)   // <-- 关键
                                 .FirstOrDefaultAsync(t => t.Id == id);
        }


        public async Task<ProjectTask?> GetByIdWithAssigneeAsync(Guid taskId)
        {
            // Include 导航属性
            return await _context.ProjectTask
                                 .Include(t => t.AssignToUser)
                                 .FirstOrDefaultAsync(t => t.Id == taskId);
        }
        //******************************ABOVE ARE NORMAL CRUD********************************//

        public async Task<int> CountTasksAsync(Guid projectId)
        {
            return await _context.ProjectTask
                .Where(t => t.ProjectId == projectId)
                .CountAsync();
        }

        public async Task<int> CountTasksByStatusAsync(Guid projectId, string status)
        {
            return await _context.ProjectTask
                .Where(t => t.ProjectId == projectId && t.Status == status)
                .CountAsync();
        }

        public async Task<int> CountTasksByStatusesAsync(Guid projectId, IEnumerable<string> statuses)
        {
            return await _context.ProjectTask
                .Where(t => t.ProjectId == projectId && statuses.Contains(t.Status))
                .CountAsync();
        }

        public async Task<int> CountOverdueTasksAsync(Guid projectId)
        {
            return await _context.ProjectTask
                .Where(t => t.ProjectId == projectId && t.DueDate < DateTime.UtcNow && t.Status != "COMPLETED")
                .CountAsync();
        }

        public async Task<Dictionary<string, int>> GetTaskCountsGroupedByStatusAsync(Guid projectId)
        {
            return await _context.ProjectTask
                .Where(t => t.ProjectId == projectId)
                .GroupBy(t => t.Status ?? "UNKNOWN")
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetTaskCountsGroupedByTypeAsync(Guid projectId)
        {
            return await _context.ProjectTask
                .Where(t => t.ProjectId == projectId)
                .GroupBy(t => t.Type ?? "UNKNOWN")  // assuming you have a 'Type' property in ProjectTask
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetTaskCountsGroupedByPriorityAsync(Guid projectId)
        {
            return await _context.ProjectTask
                .Where(t => t.ProjectId == projectId)
                .GroupBy(t => t.Priority ?? "UNKNOWN") // assuming you have a 'Priority' property
                .Select(g => new { Priority = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Priority, x => x.Count);
        }

        public async Task<int> GetTotalTasksAsync()
        {
            return await _context.ProjectTask
                .Where(t => !t.Project.IsArchived)  // ignore archived projects
                .CountAsync();
        }


        public async Task<List<ProjectTask>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<ProjectTask>();
            }

            return await _context.ProjectTask
                .Where(t =>
                    (t.Title != null && t.Title.Contains(keyword)) ||
                    (t.Description != null && t.Description.Contains(keyword))
                )
                .ToListAsync();
        }

        public async Task<int> GetMyTasksCountAsync(Guid userId)
        {
            return await _context.ProjectTask
                .Where(t => t.AssignToUserId == userId && !t.Project.IsArchived)
                .CountAsync();
        }

        public async Task<int> GetOverdueTasksCountAsync(Guid userId)
        {
            return await _context.ProjectTask
                .Where(t =>
                    t.AssignToUserId == userId &&
                    !t.Project.IsArchived &&       // ignore archived projects
                    t.DueDate < DateTime.UtcNow &&
                    t.Status != "COMPLETED")
                .CountAsync();
        }

        public async Task<int> GetOverdueTasksCountAsync()
        {
            return await _context.ProjectTask
                .Where(t =>
                    !t.Project.IsArchived &&      // ignore archived projects
                    t.DueDate < DateTime.UtcNow &&
                    t.Status != "COMPLETED")
                .CountAsync();
        }


        public async Task<Dictionary<string, int>> GetTaskCountsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ProjectTask
                .Where(t => t.DueDate != null &&
                            t.DueDate.Value.Date >= startDate.Date &&
                            t.DueDate.Value.Date <= endDate.Date)
                .GroupBy(t => t.DueDate!.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(
                    x => x.Date.ToString("yyyy-MM-dd"),
                    x => x.Count
                );
        }

        public async Task<List<ProjectTask>> GetAllOverdueTasksAsync()
        {
            var today = DateTime.UtcNow.Date;

            return await _context.ProjectTask
                .Where(t => t.DueDate != null
                            && t.DueDate < today
                            && t.Status != "COMPLETED")
                .ToListAsync();
        }


        public async Task<List<ProjectTask>> GetOverdueTasksByProjectIdAsync(Guid projectId)
        {
            var today = DateTime.UtcNow.Date;

            return await _context.ProjectTask
                .Where(t => t.ProjectId == projectId
                            && t.DueDate != null
                            && t.DueDate < today
                            && t.Status != "COMPLETED")
                .ToListAsync();
        }

        public async Task<int> CountSoonToOverdueTasksAsync()
        {
            var today = DateTime.UtcNow;
            var next7Days = today.AddDays(7);

            return await _context.ProjectTask
                .Where(t =>
                    !t.Project.IsArchived &&       // exclude archived projects
                    t.DueDate.HasValue &&
                    t.Status != "COMPLETED" &&
                    t.DueDate.Value.Date >= today &&
                    t.DueDate.Value.Date <= next7Days)
                .CountAsync();
        }

        public async Task<int> CountSoonToOverdueTasksByUserAsync(Guid userId)
        {
            var today = DateTime.UtcNow;
            var next7Days = today.AddDays(7);

            return await _context.ProjectTask
                .Where(t =>
                    !t.Project.IsArchived &&       // exclude archived projects
                    t.AssignToUserId == userId &&
                    t.DueDate.HasValue &&
                    t.Status != "COMPLETED" &&
                    t.DueDate.Value.Date >= today &&
                    t.DueDate.Value.Date <= next7Days)
                .CountAsync();
        }

        public async Task<List<ProjectTask>> GetRecentTasksAsync(int limit)
        {
            return await _context.ProjectTask
                .Include(t => t.AssignToUser)
                .Include(t => t.Project)
                .OrderByDescending(t => t.UpdatedAt)
                .Take(limit)
                .ToListAsync();
        }


        public async Task<List<ProjectTask>> GetTasksByUserAsync(Guid userId)
        {
            return await _context.ProjectTask
                .Where(t => t.AssignToUserId == userId)
                .Select(t => new ProjectTask
                {
                    Id = t.Id,
                    Title = t.Title,
                    Status = t.Status
                })
                .ToListAsync();
        }

        public async Task<List<ProjectTask>> GetByProjectIdAsync(Guid projectId)
        {
            return await _context.ProjectTask
                .Include(t => t.AssignToUser)
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<List<ProjectTask>> GetTasksByUserIdAsync(Guid userId)
        {
            return await _context.ProjectTask
                .Include(t => t.AssignToUser)
                .Where(t => t.AssignToUserId == userId)
                .ToListAsync();
        }
        public async Task<List<ProjectTask>> GetTasksByProjectAndMonthAsync(
            Guid projectId,
            int year,
            int month)
        {
            return await _context.ProjectTask
                .Include(t => t.AssignToUser)
                .Where(t =>
                    t.ProjectId == projectId &&
                    t.DueDate.HasValue &&
                    t.DueDate.Value.Year == year &&
                    t.DueDate.Value.Month == month
                )
                .ToListAsync();
        }






    }
}
