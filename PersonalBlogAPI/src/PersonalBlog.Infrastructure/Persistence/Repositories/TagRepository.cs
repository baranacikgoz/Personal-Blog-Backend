using Microsoft.EntityFrameworkCore;
using PersonalBlog.Application.Interfaces;
using PersonalBlog.Application.Interfaces.Repository;
using PersonalBlog.Domain.Entities;
using PersonalBlog.Infrastructure.Persistence.Context;
using PersonalBlog.Infrastructure.Persistence.Repositories.Common;

namespace PersonalBlog.Infrastructure.Persistence.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context, IHashIdService hashIdService) : base(context, hashIdService)
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
}
