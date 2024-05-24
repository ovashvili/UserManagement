using UserManagement.Domain.Common.Models;
using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Common.Abstractions;

public interface IRoleRepository
{
    Task<string> AddAsync(string roleName, CancellationToken cancellationToken = default);
    Task<string> DeleteAsync(string roleName, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetAllAsync(bool trackEntities = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<string> AddRoleToUserAsync(Guid userId, string roleName, CancellationToken cancellationToken = default);
    Task<string> RemoveRoleFromUserAsync(Guid userId, string roleName, CancellationToken cancellationToken = default);
}