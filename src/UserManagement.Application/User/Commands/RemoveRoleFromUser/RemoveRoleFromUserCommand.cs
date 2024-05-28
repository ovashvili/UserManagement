using MediatR;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.User.Commands.RemoveRoleFromUser;

public class RemoveRoleFromUserCommand : IRequest<Result<string>>
{
    public Guid UserId { get; set; }
    public string RoleName { get; set; } = null!;
}