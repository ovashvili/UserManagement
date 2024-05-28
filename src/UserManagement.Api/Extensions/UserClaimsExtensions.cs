using System.Security.Claims;

namespace UserManagement.Api.Extensions;

public static class UserClaimsExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(x=> x.Type == ClaimTypes.NameIdentifier);

        if (Guid.TryParse(userIdClaim?.Value, out var userId))
            return userId;

        return Guid.Empty;
    }   
}