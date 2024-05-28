using UserManagement.Application.Common.Abstractions;

namespace UserManagement.Application.Common.Services;

public class BCryptPasswordService : IPasswordService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
    }
}