using MediatR;
using UserManagement.Domain.Common.Models;
using UserManagement.Application.Common.Models.Dto;

namespace UserManagement.Application.User.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<Result<UserDto>>
{
    public Guid Id { get; set; }
}