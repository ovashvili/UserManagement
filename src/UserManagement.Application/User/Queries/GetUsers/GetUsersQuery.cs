using MediatR;
using UserManagement.Application.Common.Models;
using UserManagement.Application.Common.Models.Dto;

namespace UserManagement.Application.User.Queries.GetUsers;

public class GetUsersQuery : IRequest<Result<IEnumerable<UserDto>>>;