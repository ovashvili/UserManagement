using MediatR;
using UserManagement.Application.Common.Models;

namespace UserManagement.Application.User.Commands.AuthenticateUser;

public class AuthenticateUserCommand: IRequest<Result<AuthenticateUserResponse>>
{
    public AuthenticateUserCommandModel Model { get; set; }
}

public class AuthenticateUserResponse
{
}

public class AuthenticateUserCommandModel
{
}