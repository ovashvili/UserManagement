using UserManagement.Persistence.Context;
using System;

namespace UserManagement.Persistence.UnitOfWork;

public class UnitOfWork(UserManagementDbContext context) : IUnitOfWork, IDisposable
{
    private readonly UserManagementDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private bool _disposed;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}