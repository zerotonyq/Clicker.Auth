using System.Security.Cryptography;
using ClickerAuth.Application.AuthService.Commands.Auth.Contracts;
using ClickerAuth.Application.Jwt;
using ClickerAuth.Domain.User.Auth;
using ClickerAuth.Infrastructure.Auth;
using MediatR;

namespace ClickerAuth.Application.AuthService.Commands.SignIn;

public class SignInHandler(AuthRepository repository, JwtProvider jwtProvider)
    : IRequestHandler<SignInRequest, SignInResponse>
{
    public async Task<SignInResponse> Handle(SignInRequest request, CancellationToken cancellationToken)
    {
        var userAuthDto = await GetUser(request.Username, cancellationToken);

        if (userAuthDto == null)
            throw new NullReferenceException("Нет такого пользователя");

        if (string.IsNullOrEmpty(userAuthDto.PasswordHash))
            throw new ArgumentException("Пароль не может быть пустым");

        if (!ValidatePasswordHash(request.Password, userAuthDto.PasswordHash))
            throw new ArgumentException("Неверный пароль");

        
        var pair = await jwtProvider.GetNewPair(userAuthDto.Username, cancellationToken, userAuthDto.Roles);

        return new SignInResponse { AccessToken = pair.Item1, RefreshToken = pair.Item2 };
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

    public async Task<UserAuthDto?> GetUser(string requestUsername, CancellationToken cancellationToken)
    {
        var authDto = await repository.GetUser(requestUsername, cancellationToken);

        if (authDto == null)
            return null;

        return requestUsername != authDto.Username ? null : authDto;
    }
}