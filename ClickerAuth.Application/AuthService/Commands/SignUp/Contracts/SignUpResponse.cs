namespace ClickerAuth.Application.AuthService.Commands.SignUp.Contracts;

public class SignUpResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}