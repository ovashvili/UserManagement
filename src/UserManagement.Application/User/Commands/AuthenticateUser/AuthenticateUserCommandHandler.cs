using MediatR;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Application.Common.Models;

namespace UserManagement.Application.User.Commands.AuthenticateUser;

public class AuthenticateUserCommandHandler(IUserService userService)
    : IRequestHandler<AuthenticateUserCommand, Result<AuthenticateUserResponse>>
{
    public Task<Result<AuthenticateUserResponse>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        return userService.AuthenticateAsync(request.Model);
    }
}