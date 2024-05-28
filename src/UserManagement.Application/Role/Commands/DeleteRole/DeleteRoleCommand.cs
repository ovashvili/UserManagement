using MediatR;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.Role.Commands.DeleteRole;

public class DeleteRoleCommand : IRequest<Result<string>>
{
    public string RoleName { get; set; } = null!;
}