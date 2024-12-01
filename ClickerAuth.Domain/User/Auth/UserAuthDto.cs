namespace ClickerAuth.Domain.User.Auth;

public class UserAuthDto
{
    public required string Username { get; init; }
    public required string PasswordHash { get; init; }
    public required string[] Roles { get; init; }
    
}