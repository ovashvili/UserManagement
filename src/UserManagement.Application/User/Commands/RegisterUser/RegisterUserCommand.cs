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
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}