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
    private const string Key = "bobikBobinski228pukpuk0070000000!!!"; // ключ для шифрации
    private const int AccessLifetime = 1;
    private const int RefreshLifetime = 180;

    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.ASCII.GetBytes(Key));

    public async Task<(string access, string refresh)> GetNewPair(string username, CancellationToken cancellationToken,
        string[] roles)
    {
        var now = DateTime.UtcNow;
        
        
        var accessToken = CreateToken(now, [
            new Claim("Username", username),
            new Claim("Role", string.Join(",",roles))
        ], AccessLifetime);
        var refreshToken = CreateToken(now, [
            new Claim("Username", username),
            new Claim("Role", string.Join(",",roles))
        ], RefreshLifetime);

        var encodedAccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
        var encodedRefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);

        await refreshTokensRepository.CreateOrUpdateRefreshToken(encodedRefreshToken, cancellationToken);

        return (encodedAccessToken, encodedRefreshToken);
    }

    private static JwtSecurityToken CreateToken(DateTime startTime, List<Claim> claims,
        int lifetimeMinutes)
    {
        var accessToken = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            notBefore: startTime,
            claims: claims,
            expires: startTime.Add(TimeSpan.FromMinutes(lifetimeMinutes)),
            signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        return accessToken;
    }
}