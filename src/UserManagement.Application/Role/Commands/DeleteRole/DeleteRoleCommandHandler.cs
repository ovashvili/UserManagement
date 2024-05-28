using MediatR;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.Role.Commands.DeleteRole;

public class DeleteRoleCommandHandler(IRoleManagerService roleManagerService)
    : IRequestHandler<DeleteRoleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        return await roleManagerService.DeleteAsync(request.RoleName, cancellationToken);
    }
}