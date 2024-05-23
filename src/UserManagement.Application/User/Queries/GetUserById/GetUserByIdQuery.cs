using MediatR;
using UserManagement.Application.Common.Models;
using UserManagement.Application.Common.Models.Dto;

namespace UserManagement.Application.User.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<Result<UserDto>>
{
    public string Id { get; set; }
}