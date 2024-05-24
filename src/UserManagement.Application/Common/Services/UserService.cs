using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Application.User.Commands.AuthenticateUser;
using UserManagement.Application.User.Commands.RegisterUser;
using UserManagement.Application.User.Commands.UpdateUser;
using UserManagement.Domain.Common.Abstractions;
using UserManagement.Domain.Common.Enums;
using UserManagement.Domain.Common.Models;
using UserManagement.Infrastructure.Helpers;
using UserManagement.Infrastructure.Options;
using UserManagement.Persistence.UnitOfWork;

namespace UserManagement.Application.Common.Services;

public class UserService : IUserService
{
    private readonly IGenericRepository<Domain.Entities.User> _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly JWTAuthOptions _jwtAuthOptions;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;

    public UserService(IGenericRepository<Domain.Entities.User> userRepository, IRoleRepository roleRepository,
        IOptions<JWTAuthOptions> jwtAuthOptions, IUnitOfWork unitOfWork, IPasswordService passwordService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _jwtAuthOptions = jwtAuthOptions.Value;
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _mapper = mapper;
    }

    public async Task<Result<AuthenticateUserResponse>> AuthenticateAsync(AuthenticateUserCommandModel model,
        CancellationToken cancellationToken = default)
    { 
        var user = await _userRepository.FirstOrDefaultAsync(c => c.UserName == model.Username, cancellationToken);
        
        if (user == null || _passwordService.VerifyPassword(model.Password, user.PasswordHash))
            return Result<AuthenticateUserResponse>.Failed("Invalid username or password.", StatusCodes.Unauthorized);
        
        var userRoles = await _roleRepository.GetUserRolesAsync(user.Id, cancellationToken);
        
        var accessToken = TokenHelper.GetAccessToken(model.Username, model.Password, userRoles, _jwtAuthOptions.Secret);

        var authenticateUser = _mapper.Map<AuthenticateUserResponse>(user);

        authenticateUser.Token = accessToken;

        return Result<AuthenticateUserResponse>.Succeed(authenticateUser);
    }

    public async Task<Result<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync();

        var mappedUsers = _mapper.Map<IEnumerable<UserDto>>(users);

        return Result<IEnumerable<UserDto>>.Succeed(mappedUsers);
    }

    public async Task<Result<UserDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        
        if (user == null)
            return Result<UserDto>.Failed("User could not be found.", StatusCodes.NotFound);

        var userDto = _mapper.Map<UserDto>(user);

        return Result<UserDto>.Succeed(userDto);
    }

    public async Task<Result<UserDto>> RegisterAsync(RegisterUserCommandModel model, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.AnyAsync(e => e.UserName == model.UserName, cancellationToken))
            return Result<UserDto>.Failed("Username '" + model.UserName + "' is already taken", StatusCodes.Conflict);
        
        var user = _mapper.Map<Domain.Entities.User>(model);

        user.PasswordHash = _passwordService.HashPassword(model.Password);

        var newUser = await _userRepository.AddAsync(user, cancellationToken);
        
        await _roleRepository.AddRoleToUserAsync(newUser.Id, "Basic", cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var mappedUser = _mapper.Map<UserDto>(newUser);

        return Result<UserDto>.Succeed(mappedUser);
    }

    public async Task<Result<UserDto>> UpdateAsync(Guid id, UpdateUserCommandModel model, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (user == null)
            return Result<UserDto>.Failed("User could not be found.", StatusCodes.NotFound);

        if (await _userRepository.AnyAsync(e => e.UserName == model.UserName && e.Id != id, cancellationToken))
            return Result<UserDto>.Failed($"Username '{model.UserName}' is already taken.", StatusCodes.Conflict);
        
        user = _mapper.Map<Domain.Entities.User>(model);
        user.PasswordHash = _passwordService.HashPassword(model.Password);
        user.Id = id;
        
        var updatedUser = await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var mappedUser = _mapper.Map<UserDto>(updatedUser);
        return Result<UserDto>.Succeed(mappedUser);
    }
    
    public async Task<Result<string>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        
        if (user == null)
            return Result<string>.Failed("User could not be found.", StatusCodes.NotFound);
        
        await _userRepository.DeleteAsync(user, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Deleted");
    }
}