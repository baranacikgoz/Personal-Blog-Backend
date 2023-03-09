using Application.Interfaces.Repository.Common;
using Domain.Entities;

namespace Application.Interfaces.Repository.ReadRepositories;

public interface IArticleRepository : IGenericRepository<Article>
{
    public Task<Article> AddExistingTagToArticle(Article article, Tag tag, CancellationToken cancellationToken);

    public Task<Article?> GetByIdIncludeArticleTags(int id, CancellationToken cancellationToken);
}