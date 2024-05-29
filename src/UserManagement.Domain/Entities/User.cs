using System.Text.Json.Serialization;
using UserManagement.Domain.Common.Entities;

namespace UserManagement.Domain.Entities;

public class User : BaseEntity
{
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = null!;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = null!;

    [JsonPropertyName("userName")]
    public string UserName { get; set; } = null!;

    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [JsonPropertyName("passwordHash")]
    public string PasswordHash { get; set; } = null!;

    [JsonPropertyName("roles")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}