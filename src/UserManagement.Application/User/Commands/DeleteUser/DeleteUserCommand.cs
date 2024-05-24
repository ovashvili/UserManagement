using MediatR;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.User.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Result<string>>
{
    public Guid Id { get; set; }
}