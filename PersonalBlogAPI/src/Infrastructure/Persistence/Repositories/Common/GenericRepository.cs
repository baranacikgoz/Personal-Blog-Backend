using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Application.Interfaces.Repository.Common;
using Domain.Abstractions;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repositories.Common
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly IHashIdService _hashIdService;

        public GenericRepository(
            ApplicationDbContext context,
            IHashIdService hashIdService
            )
        {
            _context = context;
            _hashIdService = hashIdService;
        }

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            _ = await _context.Set<T>().AddAsync(entity, cancellationToken);

            entity.HashId = _hashIdService.Encode(entity.Id);

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
