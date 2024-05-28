using MediatR;
using Microsoft.AspNetCore.Http;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.Role.Queries.GetRoles;

public class GetRolesQueryHandler(IRoleManagerService roleManagerService)
    : IRequestHandler<GetRolesQuery, Result<IEnumerable<RoleDto>>>
{
    public async Task<Result<IEnumerable<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await roleManagerService.GetRolesAsync(cancellationToken);
    }
}