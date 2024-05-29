using MediatR;
using UserManagement.Domain.Common.Models;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Application.User.Commands.RegisterUser;

namespace UserManagement.Application.User.Commands.UpdateUser;

public class UpdateUserCommand: IRequest<Result<UserDto>>
{
    public Guid Id { get; set; }
    public UpdateUserCommandModel Model { get; set; }
}

public class UpdateUserCommandModel : RegisterUserCommandModel;
