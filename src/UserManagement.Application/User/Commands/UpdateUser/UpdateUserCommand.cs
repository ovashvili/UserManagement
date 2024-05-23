using MediatR;
using UserManagement.Application.Common.Models;
using UserManagement.Application.Common.Models.Dto;

namespace UserManagement.Application.User.Commands.UpdateUser;

public class UpdateUserCommand: IRequest<Result<UserDto>>
{
    public string Id { get; set; }
    public UpdateUserCommandModel Model { get; set; }
}

public class UpdateUserCommandModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}