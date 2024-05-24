using MediatR;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Domain.Common.Models;
using UserManagement.Application.Common.Models.Dto;

namespace UserManagement.Application.User.Commands.RegisterUser;

public class RegisterUserCommandHandler(IUserService userService)
    : IRequestHandler<RegisterUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        return await userService.RegisterAsync(request.Model, cancellationToken);
    }
}