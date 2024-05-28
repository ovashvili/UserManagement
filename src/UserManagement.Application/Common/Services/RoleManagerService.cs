using AutoMapper;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Application.Common.Models.Dto;
using UserManagement.Domain.Common.Abstractions;
using UserManagement.Domain.Common.Models;
using UserManagement.Persistence.UnitOfWork;

namespace UserManagement.Application.Common.Services;

public class RoleManagerService(IRoleRepository roleRepository, IUnitOfWork unitOfWork, IMapper mapper)
    : IRoleManagerService
{
    public async Task<Result<string>> AddAsync(string roleName, CancellationToken cancellationToken = default)
    {
        var result= await roleRepository.AddAsync(roleName, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed(result);
    }

    public async Task<Result<IEnumerable<RoleDto>>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var roles = await roleRepository.GetUserRolesAsync(userId, cancellationToken);
        var mappedRoles = mapper.Map<IEnumerable<RoleDto>>(roles);
        return Result<IEnumerable<RoleDto>>.Succeed(mappedRoles);
    }

    public async Task<Result<string>> AddRoleToUserAsync(Guid userId, string roleName, CancellationToken cancellationToken = default)
    {
        var result= await roleRepository.AddRoleToUserAsync(userId, roleName, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed(result);
    }

    public async Task<Result<string>> RemoveRoleFromUserAsync(Guid userId, string roleName, CancellationToken cancellationToken = default)
    {
        var result= await roleRepository.RemoveRoleFromUserAsync(userId, roleName, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed(result);
    }

    public async Task<Result<IEnumerable<RoleDto>>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = await roleRepository.GetAllAsync(false, cancellationToken);
        var mappedRoles = mapper.Map<List<RoleDto>>(roles);
        return Result<IEnumerable<RoleDto>>.Succeed(mappedRoles);
    }

    public async Task<Result<string>> DeleteAsync(string roleName, CancellationToken cancellationToken = default)
    {        
        var result = await roleRepository.DeleteAsync(roleName, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed(result);
    }
}