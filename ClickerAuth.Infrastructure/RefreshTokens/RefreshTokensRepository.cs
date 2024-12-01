using ClickerAuth.Infrastructure.Tokens.Command;
using ClickerAuth.Infrastructure.Tokens.Query;
using ClickerAuth.Shared.Postgres;
using Dapper.Transaction;

namespace ClickerAuth.Infrastructure.Tokens;

public class RefreshTokensRepository(DbConnectionFactory factory)
{
    public async Task CreateOrUpdateRefreshToken(string token, CancellationToken cancellationToken)
    {
        var command = RefreshTokensCommand.CreateOrUpdateRefreshToken(token, cancellationToken);
            
        using var connection = factory.CreateConnection();
        
        using var transaction = connection.BeginTransaction();

        await transaction.ExecuteAsync(command);
            
        transaction.Commit();
    }
    
    public async Task RevokeRefreshToken(string token, CancellationToken cancellationToken)
    {
        var command = RefreshTokensCommand.RevokeRefreshToken(token, cancellationToken);
            
        using var connection = factory.CreateConnection();
        
        using var transaction = connection.BeginTransaction();

        await transaction.ExecuteAsync(command);
            
        transaction.Commit();
    }


    public async Task<bool> CheckRefreshTokenRevoked(string requestRefreshToken, CancellationToken cancellationToken)
    {
        var command = RefreshTokensQuery.CheckRefreshTokenRevoked(requestRefreshToken, cancellationToken);
            
        using var connection = factory.CreateConnection();
        
        using var transaction = connection.BeginTransaction();

        var result = await transaction.QueryFirstOrDefaultAsync<bool>(command);
            
        transaction.Commit();

        return result;
    }
}