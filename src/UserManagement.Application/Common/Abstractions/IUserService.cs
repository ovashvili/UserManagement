using UserManagement.Domain.Common.Models;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Application.User.Commands.AuthenticateUser;
using UserManagement.Application.User.Commands.RegisterUser;
using UserManagement.Application.User.Commands.UpdateUser;

namespace UserManagement.Application.Common.Abstractions;

public interface IUserService
{
    Task<Result<AuthenticateUserResponse>> AuthenticateAsync(AuthenticateUserCommandModel model, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<UserDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> RegisterAsync(RegisterUserCommandModel model, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> UpdateAsync(Guid id, UpdateUserCommandModel model, CancellationToken cancellationToken = default);
    Task<Result<string>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}