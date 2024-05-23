using UserManagement.Application.Common.Abstractions;
using UserManagement.Application.Common.Models;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Application.User.Commands.AuthenticateUser;
using UserManagement.Application.User.Commands.RegisterUser;
using UserManagement.Application.User.Commands.UpdateUser;

namespace UserManagement.Application.Common.Services;

public class UserService : IUserService
{
    public Task<Result<AuthenticateUserResponse>> AuthenticateAsync(AuthenticateUserCommandModel model)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<UserDto>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<UserDto>> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<UserDto>> RegisterAsync(RegisterUserCommandModel model)
    {
        throw new NotImplementedException();
    }

    public Task<Result<UserDto>> UpdateAsync(string id, UpdateUserCommandModel model)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AnyAsync()
    {
        throw new NotImplementedException();
    }
}