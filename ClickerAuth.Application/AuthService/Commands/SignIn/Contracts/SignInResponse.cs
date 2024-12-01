namespace ClickerAuth.Application.AuthService.Commands.Auth.Contracts;

public class SignInResponse
{
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
}