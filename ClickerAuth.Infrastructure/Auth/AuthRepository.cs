using ClickerAuth.Domain.User.Auth;
using ClickerAuth.Infrastructure.Auth.Command;
using ClickerAuth.Infrastructure.Auth.Query;
using ClickerAuth.Shared.Postgres;
using Dapper.Transaction;

namespace ClickerAuth.Infrastructure.Auth;

public class AuthRepository(DbConnectionFactory factory)
{
    public async Task CreateOrUpdateUser(string username, string passwordHash, CancellationToken cancellationToken)
    {
        var command = AuthCommand.CreateOrUpdateUser(username, passwordHash, cancellationToken);
            
        using var connection = factory.CreateConnection();
        
        using var transaction = connection.BeginTransaction();

        await transaction.ExecuteAsync(command);
            
        transaction.Commit();
    }

    public async Task<UserAuthDto> GetUser(string requestUsername, CancellationToken cancellationToken)
    {
        var query = AuthQuery.GetUser(requestUsername, cancellationToken);
            
        using var connection = factory.CreateConnection();
        
        using var transaction = connection.BeginTransaction();

        var result = await transaction.QueryFirstOrDefaultAsync<UserAuthDto>(query);
            
        transaction.Commit();

        return result;
    }
}