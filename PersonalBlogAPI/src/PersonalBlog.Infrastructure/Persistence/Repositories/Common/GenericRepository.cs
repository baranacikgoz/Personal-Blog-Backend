using Microsoft.EntityFrameworkCore;
using PersonalBlog.Application.Interfaces.Repository.Common;
using PersonalBlog.Domain.Abstractions;
using PersonalBlog.Infrastructure.Persistence.Context;

namespace PersonalBlog.Infrastructure.Persistence.Repositories.Common
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            _ = await _context.Set<T>().AddAsync(entity, cancellationToken);

            _ = await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public virtual async Task<T> DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            _ = _context.Set<T>().Remove(entity);

            _ = await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public virtual async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AnyAsync(t => t.Id == id, cancellationToken: cancellationToken);
        }

        public virtual async Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        }

        public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            _context.Entry(entity).State = EntityState.Modified;

            _ = await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public virtual async Task<bool> ValueForThatFieldExistsAsync(string value, string fieldName, CancellationToken cancellationToken)
        {
            return await _context.Set<T>()
                .AnyAsync(e => EF.Property<string>(e, fieldName) == value, cancellationToken);
        }
    }
}