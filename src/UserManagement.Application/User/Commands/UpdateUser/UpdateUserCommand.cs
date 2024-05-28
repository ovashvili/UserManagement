using System.Text.Json.Serialization;
using MediatR;
using UserManagement.Domain.Common.Models;
using UserManagement.Application.Common.Models.Dto;

namespace UserManagement.Application.User.Commands.UpdateUser;

public class UpdateUserCommand: IRequest<Result<UserDto>>
{
    public Guid Id { get; set; }
    public UpdateUserCommandModel Model { get; set; }
}

public class UpdateUserCommandModel
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