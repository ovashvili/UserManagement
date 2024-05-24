using System.Linq.Expressions;
using UserManagement.Domain.Common.Entities;

namespace UserManagement.Domain.Common.Abstractions;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(bool trackEntities = true, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default);
}
    
