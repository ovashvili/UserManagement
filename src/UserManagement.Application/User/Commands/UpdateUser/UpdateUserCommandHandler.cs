using MediatR;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Application.Common.Models;
using UserManagement.Application.Common.Models.Dto;

namespace UserManagement.Application.User.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserService userService) : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        return await userService.UpdateAsync(request.Id, request.Model);
    }
}