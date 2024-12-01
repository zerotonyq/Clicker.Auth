using Dapper;

namespace ClickerAuth.Infrastructure.Auth.Command;

public class AuthCommand
{
    public static CommandDefinition CreateOrUpdateUser(string username, string passwordHash, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"
            insert into users
            (username, password_hash)
            values 
            (@Username, @PasswordHash)
            on conflict (username) do
            update set 
                username = @Username,
                password_hash = @PasswordHash
        ";

        var command = new CommandDefinition(sqlQuery, new
        {
            Username = username,
            PasswordHash = passwordHash
        }, cancellationToken: cancellationToken);

        return command;
    }
}