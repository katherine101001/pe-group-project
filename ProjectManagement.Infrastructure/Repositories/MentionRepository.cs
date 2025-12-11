using ProjectManagement.Domain.Entities.Collaborations;
using ProjectManagement.Domain.Interfaces.Repositories;
using ProjectManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class MentionRepository : IMentionRepository
{
    private readonly AppDbContext _context;

    public MentionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Mention>> GetByCommentIdAsync(Guid commentId)
    {
        return await _context.Set<Mention>()
                             .Where(m => m.CommentId == commentId)
                             .ToListAsync();
    }

    public async Task AddAsync(Mention mention)
    {
        _context.Set<Mention>().Add(mention);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Mention mention)
    {
        _context.Set<Mention>().Remove(mention);
        await _context.SaveChangesAsync();
    }
}
