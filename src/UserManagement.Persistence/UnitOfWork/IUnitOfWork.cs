namespace UserManagement.Persistence.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default); 
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}