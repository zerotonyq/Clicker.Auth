using Dapper;

namespace ClickerAuth.Infrastructure.Tokens.Query;

public class RefreshTokensQuery
{
    public static CommandDefinition CheckRefreshTokenRevoked(string requestRefreshToken, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
            select is_revoked from refresh_tokens where token = @Token
        ";
        var command = new CommandDefinition(sqlQuery, new
        {
            Token = requestRefreshToken
        }, cancellationToken: cancellationToken);

        return command;
    }
}