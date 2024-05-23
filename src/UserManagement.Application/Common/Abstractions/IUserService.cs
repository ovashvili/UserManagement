using UserManagement.Application.Common.Models;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Application.User.Commands.AuthenticateUser;
using UserManagement.Application.User.Commands.RegisterUser;
using UserManagement.Application.User.Commands.UpdateUser;

namespace UserManagement.Application.Common.Abstractions;

public interface IUserService
{
    Task<Result<AuthenticateUserResponse>> AuthenticateAsync(AuthenticateUserCommandModel model);
    Task<Result<IEnumerable<UserDto>>> GetAllAsync();
    Task<Result<UserDto>> GetByIdAsync(string id);
    Task<Result<UserDto>> RegisterAsync(RegisterUserCommandModel model);
    Task<Result<UserDto>> UpdateAsync(string id, UpdateUserCommandModel model);
    Task<Result<string>> DeleteAsync(string id);
}