using Application.Interfaces.Repository.Common;
using Domain.Entities;

namespace Application.Interfaces.Repository;

public interface ITagRepository : IGenericRepository<Tag>
{
    public Task<Tag?> GetByIdIncludeArticleTags(int id, CancellationToken cancellationToken);
}