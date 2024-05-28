using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserManagement.Domain.Entities;

namespace UserManagement.Persistence.Context;

public class UserManagementDbContext(IConfiguration configuration) : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserManagementDbContext).Assembly);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = configuration.GetConnectionString("UserManagementDb")!;

        optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("UserManagement.Persistence"));
    }
}