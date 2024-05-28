namespace UserManagement.Infrastructure.Options;

public class JWTAuthOptions
{
    public const string SectionName = "JWTAuth";
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}