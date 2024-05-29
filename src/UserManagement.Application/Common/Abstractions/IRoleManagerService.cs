using UserManagement.Application.Common.Models.Dto;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.Common.Abstractions;

public interface IRoleManagerService
{
    Task<Result<RoleDto>> AddAsync(string roleName, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<RoleDto>>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<string>> AddRoleToUserAsync(Guid userId, string roleName, CancellationToken cancellationToken = default);
    Task<Result<string>> RemoveRoleFromUserAsync(Guid userId, string roleName, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<RoleDto>>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<Result<string>> DeleteAsync(string roleName, CancellationToken cancellationToken = default);
}