using Domain.Abstractions;

namespace Application.Interfaces.Repository.Common
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<T> AddAsync(T entity, CancellationToken cancellationToken);

        public Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);

        public Task<T> DeleteAsync(T entity, CancellationToken cancellationToken);

        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);

        public Task<bool> ValueForThatFieldExistsAsync(string value, string fieldName, CancellationToken cancellationToken);

        public Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken);

        public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}
