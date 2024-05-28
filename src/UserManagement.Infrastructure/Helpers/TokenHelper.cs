using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Options;

namespace UserManagement.Infrastructure.Helpers;

public class TokenHelper
{
    public static string GetAccessToken(Guid userId, string userName, IEnumerable<UserRole> userRoles, JWTAuthOptions jwtAuth)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(JwtRegisteredClaimNames.Sub, userName),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };
        
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role.Role.Name)));

        foreach (var userRole in userRoles)
            claims.Add(new Claim("Role", userRole.Role.Name));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = jwtAuth.Issuer,
            Audience = jwtAuth.Audience,
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(
                key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuth.Secret)),
                algorithm: SecurityAlgorithms.HmacSha384)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}