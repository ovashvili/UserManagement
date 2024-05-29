using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Application.User.Commands.AuthenticateUser;
using UserManagement.Application.User.Commands.RegisterUser;
using UserManagement.Application.User.Commands.UpdateUser;
using UserManagement.Domain.Common.Abstractions;
using UserManagement.Domain.Common.Enums;
using UserManagement.Domain.Common.Models;
using UserManagement.Domain.Exceptions;
using UserManagement.Infrastructure.Helpers;
using UserManagement.Infrastructure.Options;
using UserManagement.Persistence.UnitOfWork;

namespace UserManagement.Application.Common.Services;

public class UserService(
    IGenericRepository<Domain.Entities.User> userRepository,
    IRoleRepository roleRepository,
    IOptions<JWTAuthOptions> jwtAuthOptions,
    IUnitOfWork unitOfWork,
    IPasswordService passwordService,
    IMapper mapper)
    : IUserService
{
    public async Task<Result<AuthenticateUserResponse>> AuthenticateAsync(Guid userId, AuthenticateUserCommandModel model,
        CancellationToken cancellationToken = default)
    { 
        if (userId != Guid.Empty)
            throw new UserIsAlreadyAuthenticatedException("User is already authenticated.");
        
        var user = await userRepository.FirstOrDefaultAsync(c => c.UserName == model.UserName,
            x => x.Include(ur => ur.UserRoles).ThenInclude(r => r.Role), cancellationToken: cancellationToken);
        
        if (user == null || !passwordService.VerifyPassword(model.Password, user.PasswordHash))
            return Result<AuthenticateUserResponse>.Failed("Invalid username or password.", StatusCodes.Unauthorized);

        var accessToken = TokenHelper.GetAccessToken(user.Id, model.UserName, user.UserRoles, jwtAuthOptions.Value);

        var authenticateUser = mapper.Map<AuthenticateUserResponse>(user);

        authenticateUser.Token = accessToken;

        return Result<AuthenticateUserResponse>.Succeed(authenticateUser);
    }

    public async Task<Result<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllAsync(cancellationToken: cancellationToken);

        var mappedUsers = mapper.Map<IEnumerable<UserDto>>(users);

        return Result<IEnumerable<UserDto>>.Succeed(mappedUsers);
    }

    public async Task<Result<UserDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        
        if (user == null)
            return Result<UserDto>.Failed("User could not be found.", StatusCodes.NotFound);

        var userDto = mapper.Map<UserDto>(user);

        return Result<UserDto>.Succeed(userDto);
    }

    public async Task<Result<UserDto>> RegisterAsync(RegisterUserCommandModel model, CancellationToken cancellationToken = default)
    {
        if (await userRepository.AnyAsync(e => e.UserName == model.UserName, cancellationToken))
            return Result<UserDto>.Failed("Username '" + model.UserName + "' is already taken", StatusCodes.Conflict);
        
        var user = mapper.Map<Domain.Entities.User>(model);

        user.PasswordHash = passwordService.HashPassword(model.Password);

        var newUser = await userRepository.AddAsync(user, cancellationToken);
        
        await roleRepository.AddRoleToUserAsync(newUser.Id, "User", cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var mappedUser = mapper.Map<UserDto>(newUser);

        return Result<UserDto>.Succeed(mappedUser);
    }

    public async Task<Result<UserDto>> UpdateAsync(Guid id, UpdateUserCommandModel model, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user == null)
            return Result<UserDto>.Failed("User could not be found.", StatusCodes.NotFound);

        if (await userRepository.AnyAsync(e => e.UserName == model.UserName && e.Id != id, cancellationToken))
            return Result<UserDto>.Failed($"Username '{model.UserName}' is already taken.", StatusCodes.Conflict);
        
        user = mapper.Map<Domain.Entities.User>(model);
        user.PasswordHash = passwordService.HashPassword(model.Password);
        user.Id = id;
        
        var updatedUser = await userRepository.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var mappedUser = mapper.Map<UserDto>(updatedUser);
        return Result<UserDto>.Succeed(mappedUser);
    }
    
    public async Task<Result<string>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        
        if (user == null)
            return Result<string>.Failed("User could not be found.", StatusCodes.NotFound);
        
        await userRepository.DeleteAsync(user, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Deleted Successfully.");
    }
}