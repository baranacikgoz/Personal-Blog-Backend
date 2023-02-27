using Microsoft.EntityFrameworkCore;
using PersonalBlog.Application.Caching;
using PersonalBlog.Application.Interfaces.Repository;
using PersonalBlog.Domain.Entities;
using PersonalBlog.Infrastructure.Persistence.Context;

namespace PersonalBlog.Infrastructure.Persistence.Repositories.ReadRepositories;

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
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}