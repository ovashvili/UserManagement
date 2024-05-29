using MediatR;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.Role.Commands.CreateRole;

public class CreateRoleCommandHandler(IRoleManagerService roleManagerService)
    : IRequestHandler<CreateRoleCommand, Result<RoleDto>>
{
    public async Task<Result<RoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        return await roleManagerService.AddAsync(request.Model.RoleName, cancellationToken);
    }
}