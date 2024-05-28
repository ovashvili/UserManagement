using MediatR;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.User.Queries.GetUserRoles;

public class GetUserRolesQuery : IRequest<Result<IEnumerable<RoleDto>>>
{
    public Guid UserId { get; set; }
}