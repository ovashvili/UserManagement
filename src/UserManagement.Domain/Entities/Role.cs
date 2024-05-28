using System.Text.Json.Serialization;
using UserManagement.Domain.Common.Entities;

namespace UserManagement.Domain.Entities;

public class Role : BaseEntity
{
    [JsonPropertyName("roleName")]
    public string Name { get; set; } = null!;
    
    [JsonPropertyName("users")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}