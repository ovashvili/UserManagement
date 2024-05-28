using MediatR;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.User.Queries.GetUserRoles;

public class GetUserRolesQueryHandler(IRoleManagerService roleManagerService) : IRequestHandler<GetUserRolesQuery, Result<IEnumerable<RoleDto>>>
{
    public async Task<Result<IEnumerable<RoleDto>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        return await roleManagerService.GetUserRolesAsync(request.UserId, cancellationToken);
    }
}