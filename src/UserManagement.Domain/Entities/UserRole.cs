using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Domain.Entities;

public class UserRole
{
    [Key]
    [Column(Order = 1)]
    public Guid UserId { get; set; }
    
    [Key]
    [Column(Order = 2)]
    public User User { get; set; } = null!;

    // Navigation properties
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}