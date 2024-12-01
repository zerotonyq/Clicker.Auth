using ClickerAuth.Shared.Postgres;
using ClickerAuth.Shared.Postgres.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClickerAuth.Shared;

public static class SharedInjection
{
    public static IServiceCollection Configure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseConfiguration>(configuration.GetSection(nameof( DatabaseConfiguration )));
        services.AddSingleton<DbConnectionFactory>();
        
        return services;
    }
}