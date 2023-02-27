using PersonalBlog.Application.Interfaces.Repository.Common;
using PersonalBlog.Domain.Entities;

namespace PersonalBlog.Application.Interfaces.Repository;

public interface ITagRepository : IGenericRepository<Tag>
{
    public Task<Tag?> GetByIdIncludeArticleTags(int id, CancellationToken cancellationToken);
}