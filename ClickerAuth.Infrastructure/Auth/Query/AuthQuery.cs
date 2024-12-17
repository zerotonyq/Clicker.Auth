using ClickerAuth.Domain.User.Auth;
using Dapper;

namespace ClickerAuth.Infrastructure.Auth.Query;

public static class AuthQuery
{
    public static CommandDefinition GetUser(string requestUsername, CancellationToken cancellationToken)
    {
        const string sqlQuery = @$"
            select 
                   users.id as {nameof(UserAuthDto.Id)},
                   username as {nameof(UserAuthDto.Username)}, 
                   password_hash as {nameof(UserAuthDto.PasswordHash)},
                   array_agg(r.role) as {nameof(UserAuthDto.Roles)}
            from users 
                left join user_roles ur on ur.user_id = users.id
                left join roles r on r.id = ur.role_id
            where username = @Username 
            group by users.id, username, password_hash limit 1 
        ";

        var command = new CommandDefinition(sqlQuery, new
        {
            Username = requestUsername
        }, cancellationToken: cancellationToken);

        return command;
    }
}