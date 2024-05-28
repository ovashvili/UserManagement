using MediatR;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.User.Commands.RemoveRoleFromUser;

public class RemoveRoleFromUserCommandHandler(IRoleManagerService roleManagerService)
    : IRequestHandler<RemoveRoleFromUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        return await roleManagerService.RemoveRoleFromUserAsync(request.UserId, request.RoleName, cancellationToken);
    }
}