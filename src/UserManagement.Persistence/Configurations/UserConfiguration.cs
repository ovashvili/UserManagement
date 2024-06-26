using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Entities;

namespace UserManagement.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> modelBuilder)
    {
        modelBuilder.ToTable("Users");

        modelBuilder.HasKey(u => u.Id);

        modelBuilder.Property(u => u.FirstName)
            .IsUnicode(false)
            .HasMaxLength(60);

        modelBuilder.Property(u => u.LastName)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(80);

        modelBuilder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(32);

        modelBuilder.HasIndex(u => u.UserName)
            .IsUnique();

        modelBuilder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        modelBuilder.HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Property(u => u.PasswordHash)
            .IsRequired();

        modelBuilder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}