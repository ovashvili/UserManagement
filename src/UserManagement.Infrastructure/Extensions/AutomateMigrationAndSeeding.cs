using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserManagement.Domain.Common.Abstractions;
using UserManagement.Domain.Entities;
using UserManagement.Persistence.Context;
using UserManagement.Persistence.UnitOfWork;

namespace UserManagement.Infrastructure.Extensions;

public static class MigrationAndSeedingExtension
{
    public static async Task AutomateMigrationAndSeeding(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<UserManagementDbContext>();
        var userRepository = services.GetRequiredService<IGenericRepository<User>>();
        var roleRepository = services.GetRequiredService<IRoleRepository>();
        var unitOfWork = services.GetRequiredService<IUnitOfWork>();

        // Migrate the database
        await context.Database.MigrateAsync();

        // Seed roles
        await ContextSeed.SeedRolesAsync(roleRepository, unitOfWork);

        // Seed sudo user
        await ContextSeed.SeedSudoAsync(userRepository, roleRepository, unitOfWork);
    }
}