using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClickerAuth.Infrastructure.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace ClickerAuth.Application.Jwt;

public class JwtProvider(RefreshTokensRepository refreshTokensRepository)
{
    public const string Issuer = "Zerotonyq"; // издатель токена
    public const string Audience = "WhoseHamster"; // потребитель токена
    private const string Key = "bobikBobinski228pukpuk0070000000!!!";   // ключ для шифрации
    private const int AccessLifetime = 1;
    private const int RefreshLifetime = 180;
    
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.ASCII.GetBytes(Key));
    
    public async Task<(string access, string refresh)> GetNewPair(string username, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        
        var accessToken = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            notBefore: now,
            claims: new[] { new Claim("Username", username) },
            expires: now.Add(TimeSpan.FromMinutes(AccessLifetime)),
            signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        
        var refreshToken = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            notBefore: now,
            claims: new[] { new Claim("Username", username) },
            expires: now.Add(TimeSpan.FromMinutes(RefreshLifetime)),
            signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        var encodedAccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
        var encodedRefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);
        
        await refreshTokensRepository.CreateOrUpdateRefreshToken(encodedRefreshToken, cancellationToken);

        return (encodedAccessToken, encodedRefreshToken);
    }
}