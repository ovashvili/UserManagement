using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace UserManagement.Infrastructure.Helpers;

public class TokenHelper
{
    public static string GetAccessToken(string userName, string password, IEnumerable<string> userRoles, string secret)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userName),
        };

        foreach (var role in userRoles)
            claims.Add(new Claim("Role", role));

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "jwt",
            audience: "jwt-audience",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: new SigningCredentials(
                key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                algorithm: SecurityAlgorithms.HmacSha384
            )
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}