using ClickerAuth.Infrastructure.Auth;
using ClickerAuth.Infrastructure.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace ClickerAuth.Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection Configure(this IServiceCollection services)
    {
        services.AddSingleton<AuthRepository>();
        services.AddSingleton<RefreshTokensRepository>();
        return services;
    }
}