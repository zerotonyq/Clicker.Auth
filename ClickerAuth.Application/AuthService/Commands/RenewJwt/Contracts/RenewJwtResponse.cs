namespace ClickerAuth.Application.AuthService.Commands.RenewJwt.Contracts;

public class RenewJwtResponse
{
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
}