using UserManagement.Domain.Common.Abstractions;
using UserManagement.Domain.Entities;
using UserManagement.Persistence.Repositories;
using UserManagement.Persistence.UnitOfWork;

namespace UserManagement.Persistence.Context;

public class ContextSeed
{
    public static async Task SeedRolesAsync(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        if (!await roleRepository.AnyAsync())
        {
            await roleRepository.AddAsync("Admin");
            await roleRepository.AddAsync("User");
            await unitOfWork.SaveChangesAsync();
        }
    }

public static async Task SeedSudoAsync(IGenericRepository<User> userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        if (!await userRepository.AnyAsync(u => u.UserName == "John3"))
        {
            var user = new User
            {
                FirstName = "John", 
                LastName = "Doe", 
                UserName = "John3", 
                Email = "johnDoe421@example.com",
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("{E=G(AhM(8")
            };
            var response = await userRepository.AddAsync(user);
            await roleRepository.AddRoleToUserAsync(response.Id, "Admin");
            await roleRepository.AddRoleToUserAsync(response.Id, "User");
            await unitOfWork.SaveChangesAsync();
        }
    }
}