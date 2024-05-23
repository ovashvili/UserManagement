using MediatR;
using UserManagement.Application.Common.Models;

namespace UserManagement.Application.User.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Result<string>>
{
    public string Id { get; set; }
}