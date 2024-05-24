using MediatR;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.User.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserService userService) : IRequestHandler<DeleteUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        return await userService.DeleteAsync(request.Id, cancellationToken);
    }
}