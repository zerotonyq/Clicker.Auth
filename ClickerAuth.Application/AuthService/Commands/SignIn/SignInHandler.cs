using System.Security.Cryptography;
using ClickerAuth.Application.AuthService.Commands.Auth.Contracts;
using ClickerAuth.Application.Jwt;
using ClickerAuth.Domain.User.Auth;
using ClickerAuth.Infrastructure.Auth;
using MediatR;

namespace ClickerAuth.Application.AuthService.Commands.SignIn;

public class SignInHandler(AuthRepository repository, JwtProvider jwtProvider) : IRequestHandler<SignInRequest, SignInResponse>
{  
    public async Task<SignInResponse> Handle(SignInRequest request, CancellationToken cancellationToken)
    {
        var userAuthDto =await GetUser(request.Username, request.Password, cancellationToken);

        if (userAuthDto == null)
            throw new NullReferenceException("There is no user with such credentials");

        var pair =await jwtProvider.GetNewPair(userAuthDto.Username, cancellationToken);

        return new SignInResponse { AccessToken = pair.Item1, RefreshToken = pair.Item2};
    }

    private bool ValidatePasswordHash(string password, string passwordHash)
    {
        var hashBytes = Convert.FromBase64String(passwordHash);

        var salt = new byte[16];
        
        Array.Copy(hashBytes, 0, salt, 0, 16);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);

        var hash = pbkdf2.GetBytes(20);

        for (int i = 0; i < 20; i++)
            if (hashBytes[i + 16] != hash[i])
                return false;

        return true;
    }

    private async Task<UserAuthDto?> GetUser(string requestUsername, string requestPassword, CancellationToken cancellationToken)
    {
        var authDto = await repository.GetUser(requestUsername, cancellationToken);

        if (authDto == null || string.IsNullOrEmpty(authDto.PasswordHash))
            return null;
            
        if (!ValidatePasswordHash(requestPassword, authDto.PasswordHash))
            return null;

        return requestUsername != authDto.Username ? null : authDto;
    }

}