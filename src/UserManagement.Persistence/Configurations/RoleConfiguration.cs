using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Entities;

namespace UserManagement.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> modelBuilder)
    {
        modelBuilder.ToTable("Roles");

        modelBuilder.HasKey(r => r.Id);

        modelBuilder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(32);

        modelBuilder.HasIndex(r => r.Name)
            .IsUnique();

        modelBuilder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}