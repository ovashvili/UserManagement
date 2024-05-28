using System.Text.Json.Serialization;
using MediatR;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.User.Commands.AddRoleToUser;

public class AddRoleToUserCommand : IRequest<Result<string>>
{
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }
    public AddRoleToUserCommandModel Model { get; set; }

}

public class AddRoleToUserCommandModel
{
    [JsonPropertyName("roleName")]
    public string RoleName { get; set; } = null!;
}