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
    public async Task<Result<RoleDto>> AddAsync(string roleName, CancellationToken cancellationToken = default)
    {
        var result = await roleRepository.AddAsync(roleName, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var mappedResult = mapper.Map<RoleDto>(result);
        return Result<RoleDto>.Succeed(mappedResult);
    }

    public async Task<Result<IEnumerable<RoleDto>>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await roleRepository.GetUserRolesAsync(userId, cancellationToken);
        var mappedResult = mapper.Map<IEnumerable<RoleDto>>(result);
        return Result<IEnumerable<RoleDto>>.Succeed(mappedResult);
    }

    public async Task<Result<string>> AddRoleToUserAsync(Guid userId, string roleName, CancellationToken cancellationToken = default)
    {
        await roleRepository.AddRoleToUserAsync(userId, roleName, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Assigned Successfully.");
    }

    public async Task<Result<string>> RemoveRoleFromUserAsync(Guid userId, string roleName, CancellationToken cancellationToken = default)
    {
        await roleRepository.RemoveRoleFromUserAsync(userId, roleName, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Deleted Successfully.");
    }

    public async Task<Result<IEnumerable<RoleDto>>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        var result = await roleRepository.GetAllAsync(false, cancellationToken);
        var mappedResult = mapper.Map<List<RoleDto>>(result);
        return Result<IEnumerable<RoleDto>>.Succeed(mappedResult);
    }

    public async Task<Result<string>> DeleteAsync(string roleName, CancellationToken cancellationToken = default)
    {        
        await roleRepository.DeleteAsync(roleName, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Deleted Successfully.");
    }
}