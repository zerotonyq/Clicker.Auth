using ClickerAuth.Application.AuthService.Commands.Auth;
using ClickerAuth.Application.AuthService.Commands.RenewJwt;
using ClickerAuth.Application.AuthService.Commands.SignIn;
using ClickerAuth.Application.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ClickerAuth.Application;

public static class ApplicationInjection
{
    public static IServiceCollection Configure(this IServiceCollection services)
    {
        services.AddSingleton<SignInHandler>();
        services.AddSingleton<SignUpHandler>();
        services.AddSingleton<RenewJwtHandler>();
        services.AddSingleton<JwtProvider>();
        
        AddJwtAuth(services);
        
        return services;
    }

    private static void AddJwtAuth(IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = JwtProvider.Issuer,
 
                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = JwtProvider.Audience,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,
 
                    // установка ключа безопасности
                    IssuerSigningKey = JwtProvider.GetSymmetricSecurityKey(),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };
            });
    }
}