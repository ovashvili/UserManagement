using MediatR;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.Role.Commands.CreateRole;

public class CreateRoleCommandHandler(IRoleManagerService roleManagerService)
    : IRequestHandler<CreateRoleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        return await roleManagerService.AddAsync(request.Model.RoleName, cancellationToken);
    }
}