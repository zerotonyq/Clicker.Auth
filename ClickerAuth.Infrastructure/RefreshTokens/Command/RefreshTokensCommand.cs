using Dapper;

namespace ClickerAuth.Infrastructure.Tokens.Command;

public class RefreshTokensCommand
{
    public static CommandDefinition CreateOrUpdateRefreshToken(string token, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
            insert into refresh_tokens
            (token, is_revoked)
            values 
            (@Token, false)
        ";
        var command = new CommandDefinition(sqlQuery, new
        {
            Token = token
        }, cancellationToken: cancellationToken);

        return command;
    }

    public static CommandDefinition RevokeRefreshToken(string token, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
            update refresh_tokens set is_revoked = true where token = @Token 
        ";
        var command = new CommandDefinition(sqlQuery, new
        {
            Token = token
        }, cancellationToken: cancellationToken);

        return command;
    }
}