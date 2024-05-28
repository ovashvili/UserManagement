using System.Text.Json.Serialization;
using MediatR;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.User.Commands.AuthenticateUser;

public class AuthenticateUserCommand : IRequest<Result<AuthenticateUserResponse>>
{
    public Guid UserId { get; set; }
    public AuthenticateUserCommandModel Model { get; set; }
}

public class AuthenticateUserResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
}

public class AuthenticateUserCommandModel
{
    [JsonPropertyName("userName")] 
    public string UserName { get; set; } = null!;
    [JsonPropertyName("password")] 
    public string Password { get; set; } = null!;
}