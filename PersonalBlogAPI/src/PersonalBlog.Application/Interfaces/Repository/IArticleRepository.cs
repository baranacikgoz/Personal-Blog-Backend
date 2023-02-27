using PersonalBlog.Application.Interfaces.Repository.Common;
using PersonalBlog.Domain.Entities;

namespace PersonalBlog.Application.Interfaces.Repository.ReadRepositories;

public interface IArticleRepository : IGenericRepository<Article>
{
    public Task<Article> AddExistingTagToArticle(Article article, Tag tag, CancellationToken cancellationToken);

    public Task<Article?> GetByIdIncludeArticleTags(int id, CancellationToken cancellationToken);
}