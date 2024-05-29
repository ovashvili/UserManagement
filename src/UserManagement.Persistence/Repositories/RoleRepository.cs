using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Common.Abstractions;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Exceptions;
using UserManagement.Persistence.Context;

namespace UserManagement.Persistence.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DbSet<Role> _roleDbSet;
        private readonly DbSet<UserRole> _userRoleDbSet;
        private readonly UserManagementDbContext _context;

        public RoleRepository(UserManagementDbContext context)
        {
            _context = context;
            _roleDbSet = _context.Set<Role>();
            _userRoleDbSet = _context.Set<UserRole>();
        }

        public async Task<Role> AddAsync(string roleName, CancellationToken cancellationToken = default)
        {
            var role = new Role { Name = roleName };
            var entityEntry = await _roleDbSet.AddAsync(role, cancellationToken);
            return entityEntry.Entity;
        }

        public async Task DeleteAsync(string roleName, CancellationToken cancellationToken = default)
        {
            var role = await _roleDbSet.SingleOrDefaultAsync(r => r.Name == roleName, cancellationToken);
            
            if (role == null)
                throw new EntityNotFoundException($"Role not found. RoleName: {roleName}");

            _roleDbSet.Remove(role);
        }

        public async Task<IEnumerable<Role>> GetAllAsync(bool trackEntities = true,
            CancellationToken cancellationToken = default)
        {
            return trackEntities
                ? await _roleDbSet.ToListAsync(cancellationToken)
                : await _roleDbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _userRoleDbSet
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role).ToListAsync(cancellationToken);
        }

        public async Task AddRoleToUserAsync(Guid userId, string roleName,
            CancellationToken cancellationToken = default)
        {
            var role = await _roleDbSet.SingleOrDefaultAsync(r => r.Name == roleName, cancellationToken);
            
            if (role == null)
                throw new EntityNotFoundException($"User role not found. UserId: {userId}, RoleName: {roleName}");

            var userRole = new UserRole { UserId = userId, RoleId = role.Id };
            await _userRoleDbSet.AddAsync(userRole, cancellationToken);
        }

        public async Task RemoveRoleFromUserAsync(Guid userId, string roleName,
            CancellationToken cancellationToken = default)
        {
            var role = await _roleDbSet.SingleOrDefaultAsync(r => r.Name == roleName, cancellationToken);
          
            if (role == null)
                throw new EntityNotFoundException($"Role not found. RoleName: {roleName}");

            var userRole = await _userRoleDbSet
                .SingleOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == role.Id, cancellationToken);
            
            if (userRole == null)
                throw new EntityNotFoundException($"User role not found. UserId: {userId}, RoleName: {roleName}");

            _userRoleDbSet.Remove(userRole);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {        
            return await _roleDbSet.AsQueryable().AnyAsync(cancellationToken);
        }
    }
}