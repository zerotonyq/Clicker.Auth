using System.Security.Cryptography;
using ClickerAuth.Application.AuthService.Commands.Auth;
using ClickerAuth.Application.AuthService.Commands.Auth.Contracts;
using ClickerAuth.Application.AuthService.Commands.SignIn.Contracts;
using ClickerAuth.Application.AuthService.Commands.SignUp.Contracts;
using ClickerAuth.Infrastructure.Auth;
using MediatR;
using SignInRequest = ClickerAuth.Application.AuthService.Commands.Auth.Contracts.SignInRequest;

namespace ClickerAuth.Application.AuthService.Commands.SignIn;

public class SignUpHandler(
    AuthRepository repository,
    SignInHandler signInHandler) : IRequestHandler<Contracts.SignUpRequest, SignUpResponse>
{
    public async Task<SignUpResponse> Handle(Contracts.SignUpRequest request, CancellationToken cancellationToken)
    {
        byte[] salt;
        RandomNumberGenerator.Create().GetBytes(salt = new byte[16]);

        var pbkdf2 = new Rfc2898DeriveBytes(request.Password, salt, 10000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(20);

        var hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        var savedPasswordHash = Convert.ToBase64String(hashBytes);

        await repository.CreateOrUpdateUser(request.Username, savedPasswordHash, cancellationToken);

        var authResult =
            await signInHandler.Handle(new SignInRequest(request.Username, request.Password), cancellationToken);

        return new SignUpResponse() { AccessToken = authResult.AccessToken, RefreshToken = authResult.RefreshToken };
    }
}