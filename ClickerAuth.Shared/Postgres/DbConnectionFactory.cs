using System.Data;
using ClickerAuth.Shared.Postgres.Config;
using Microsoft.Extensions.Options;
using Npgsql;

namespace ClickerAuth.Shared.Postgres;

public class DbConnectionFactory(IOptionsMonitor<DatabaseConfiguration> dbSettings)
{
    public IDbConnection CreateConnection()
    {
        var connectionString = CreateConnectionString(dbSettings.CurrentValue);

        var connection = new NpgsqlConnection(connectionString);
        
        connection.Open();

        return connection;
    }

    private static string CreateConnectionString(DatabaseConfiguration dbSettingsCurrentValue)
    {
        return $"Host={dbSettingsCurrentValue.Server}; " +
               $"Database={dbSettingsCurrentValue.Database}; " +
               $"Username={dbSettingsCurrentValue.Username}; " +
               $"Password={dbSettingsCurrentValue.Password}; ";
    }
}