using System.Text.Json.Serialization;
using MediatR;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.Role.Commands.CreateRole;

public class CreateRoleCommand : IRequest<Result<string>>
{
    public CreateRoleCommandModel Model { get; set; }
}

public class CreateRoleCommandModel
{
        
    [JsonPropertyName("roleName")] 
    public string RoleName { get; set; } = null!;
}