using MediatR;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Domain.Common.Models;
using UserManagement.Application.Common.Models.Dto;

namespace UserManagement.Application.User.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserService userService) : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await userService.GetByIdAsync(request.Id, cancellationToken);
    }
}