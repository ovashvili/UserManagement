namespace UserManagement.Application.Common.Abstractions;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}