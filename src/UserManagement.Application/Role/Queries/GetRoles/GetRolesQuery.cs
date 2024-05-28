using MediatR;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.Role.Queries.GetRoles;

public class GetRolesQuery : IRequest<Result<IEnumerable<RoleDto>>>;