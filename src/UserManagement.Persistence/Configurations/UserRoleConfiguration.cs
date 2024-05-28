using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Entities;

namespace UserManagement.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> modelBuilder)
    {
        modelBuilder.ToTable("UserRoles");
            
        modelBuilder.HasKey(ur => new { ur.UserId, ur.RoleId });
    }
}