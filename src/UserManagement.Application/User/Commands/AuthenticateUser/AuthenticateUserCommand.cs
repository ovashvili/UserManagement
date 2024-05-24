using MediatR;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.User.Commands.AuthenticateUser;

public class AuthenticateUserCommand : IRequest<Result<AuthenticateUserResponse>>
{
    public AuthenticateUserCommandModel Model { get; set; }
}

public class AuthenticateUserResponse
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string Lastname { get; set; }
    public string UserName { get; set; }
    public string Token { get; set; }
}

public class AuthenticateUserCommandModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}