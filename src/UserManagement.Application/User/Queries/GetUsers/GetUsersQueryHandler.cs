using MediatR;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Application.Common.Models;
using UserManagement.Application.Common.Models.Dto;

namespace UserManagement.Application.User.Queries.GetUsers;

public class GetUsersQueryHandler(IUserService userService)
    : IRequestHandler<GetUsersQuery, Result<IEnumerable<UserDto>>>
{
    public async Task<Result<IEnumerable<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await userService.GetAllAsync();
    }
}