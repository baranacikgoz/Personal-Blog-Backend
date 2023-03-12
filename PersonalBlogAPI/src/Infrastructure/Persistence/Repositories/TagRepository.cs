using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories.Common;

namespace Infrastructure.Persistence.Repositories;

public class TagRepository : GenericRepository<Tag>, ITagRepository
{
    private readonly ApplicationDbContext _context;

    public TagRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Tag?> GetByIdIncludeArticleTags(int id, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .Include(t => t.ArticleTags)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);
    }
}
