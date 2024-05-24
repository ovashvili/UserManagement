using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Common.Abstractions;
using UserManagement.Domain.Common.Entities;
using UserManagement.Domain.Exceptions;
using UserManagement.Persistence.Context;

namespace UserManagement.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet;

    public GenericRepository(UserManagementDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbSet = dbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(id, CancellationToken.None);
    }

    public async Task<IEnumerable<T>> GetAllAsync(bool trackEntities = true, CancellationToken cancellationToken = default)
    {
        return trackEntities ? await _dbSet.ToListAsync(CancellationToken.None) : await _dbSet.AsNoTracking().ToListAsync(CancellationToken.None);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var entityEntry = await _dbSet.AddAsync(entity, CancellationToken.None);
        return entityEntry.Entity;
    }

    public Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var updatedEntityEntry = _dbSet.Update(entity);
        return Task.FromResult(updatedEntityEntry.Entity);
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, CancellationToken.None);
            
        if (entity == null)
            throw new EntityNotFoundException($"Entity with ID '{id}' not found.");
            
        await DeleteAsync(entity, CancellationToken.None);
    }
    
    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsQueryable().FirstOrDefaultAsync(condition, cancellationToken);
    }
    
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsQueryable().AnyAsync(condition, cancellationToken);
    }
}