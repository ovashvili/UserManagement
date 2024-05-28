using System.Text.Json.Serialization;
using MediatR;
using UserManagement.Domain.Common.Models;
using UserManagement.Application.Common.Models.Dto;

namespace UserManagement.Application.User.Commands.RegisterUser;

public class RegisterUserCommand: IRequest<Result<UserDto>>
{
    public RegisterUserCommandModel Model { get; set; }
}

public class RegisterUserCommandModel
{
    [JsonPropertyName("firstName")] 
    public string FirstName { get; set; } = null!;
    [JsonPropertyName("lastName")] 
    public string LastName { get; set; } = null!;
    [JsonPropertyName("userName")] 
    public string UserName { get; set; } = null!;

    [JsonPropertyName("email")] 
    public string Email { get; set; } = null!;
    [JsonPropertyName("password")] 
    public string Password { get; set; } = null!;
}