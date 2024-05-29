using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Common.Abstractions;

public interface IRoleRepository
{
    Task<Role> AddAsync(string roleName, CancellationToken cancellationToken = default);
    Task DeleteAsync(string roleName, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetAllAsync(bool trackEntities = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddRoleToUserAsync(Guid userId, string roleName, CancellationToken cancellationToken = default);
    Task RemoveRoleFromUserAsync(Guid userId, string roleName, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
}