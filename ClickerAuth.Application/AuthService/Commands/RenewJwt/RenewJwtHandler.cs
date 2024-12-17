using ClickerAuth.Application.AuthService.Commands.RenewJwt.Contracts;
using ClickerAuth.Application.AuthService.Commands.SignIn;
using ClickerAuth.Application.Jwt;
using ClickerAuth.Infrastructure.Tokens;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ClickerAuth.Application.AuthService.Commands.RenewJwt;

public class RenewJwtHandler(SignInHandler signInHandler, RefreshTokensRepository refreshTokensRepository, JwtProvider jwtProvider)
    : IRequestHandler<RenewJwtRequest, RenewJwtResponse>
{
    public async Task<RenewJwtResponse> Handle(RenewJwtRequest request, CancellationToken cancellationToken)
    {
        var isRevoked = await refreshTokensRepository.CheckRefreshTokenRevoked(request.RefreshToken, cancellationToken);
        
        if (isRevoked)
            throw new SecurityTokenException("This token is revoked");
        
        var userAuthDto =await signInHandler.GetUser(request.Username, cancellationToken);
        
        if(userAuthDto == null)
            throw new NullReferenceException("Нет такого пользователя");

        var pair =await jwtProvider.GetNewPair(request.Username, cancellationToken, userAuthDto.Roles, userAuthDto.Id);
        
        await refreshTokensRepository.RevokeRefreshToken(request.RefreshToken, cancellationToken);
        
        return new RenewJwtResponse()
        {
            RefreshToken = pair.refresh,
            AccessToken = pair.access
        };
    }
}