using Microsoft.EntityFrameworkCore;
using PersonalBlog.Application.Interfaces.Repository.ReadRepositories;
using PersonalBlog.Domain.Entities;
using PersonalBlog.Infrastructure.Persistence.Context;
using PersonalBlog.Infrastructure.Persistence.Repositories.Common;

namespace PersonalBlog.Infrastructure.Persistence.Repositories
{
    public class ArticleRepository : GenericRepository<Article>, IArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public ArticleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Article> AddExistingTagToArticle(Article article, Tag tag, CancellationToken cancellationToken)
        {
            ArticleTag articleTag = new()
            {
                ArticleId = article.Id,
                TagId = tag.Id
            };

            article.ArticleTags.Add(articleTag);
            tag.ArticleTags.Add(articleTag);

            _ = await _context.SaveChangesAsync(cancellationToken);

            return article;
        }

        public async Task<Article?> GetByIdIncludeArticleTags(int id, CancellationToken cancellationToken)
        {
            return await _context.Articles
                .Include(a => a.ArticleTags)
                .ThenInclude(at => at.Tag)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }
    }
}