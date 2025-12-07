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
            return await _context.ProjectTask.ToListAsync();
        }

        public async Task<ProjectTask?> GetByIdAsync(Guid id)
        {
            return await _context.ProjectTask.FindAsync(id);
        }
    }
}
