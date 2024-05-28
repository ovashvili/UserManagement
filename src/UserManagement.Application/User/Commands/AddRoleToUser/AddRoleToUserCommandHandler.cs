using MediatR;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.User.Commands.AddRoleToUser;

public class AddRoleToUserCommandHandler(IRoleManagerService roleManagerService)
    : IRequestHandler<AddRoleToUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
    {
        return await roleManagerService.AddRoleToUserAsync(request.UserId, request.Model.RoleName, cancellationToken);
    }
}