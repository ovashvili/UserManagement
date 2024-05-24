using MediatR;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Domain.Common.Models;

namespace UserManagement.Application.User.Queries.GetUsers;

public class GetUsersQuery : IRequest<Result<IEnumerable<UserDto>>>;