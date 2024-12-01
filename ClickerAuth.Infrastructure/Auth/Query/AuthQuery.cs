using ClickerAuth.Domain.User.Auth;
using Dapper;

namespace ClickerAuth.Infrastructure.Auth.Query;

public static class AuthQuery
{
    public static CommandDefinition GetUser(string requestUsername, CancellationToken cancellationToken)
    {
        const string sqlQuery = @$"
            select username as {nameof(UserAuthDto.Username)}, 
                   password_hash as {nameof(UserAuthDto.PasswordHash)}
            from users
            where username = @Username limit 1
        ";

        var command = new CommandDefinition(sqlQuery, new
        {
            Username = requestUsername
        }, cancellationToken: cancellationToken);

        return command;
    }
}