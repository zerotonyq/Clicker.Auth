using ClickerAuth.Application.AuthService.Commands.RenewJwt.Contracts;
using ClickerAuth.Application.Jwt;
using ClickerAuth.Infrastructure.Tokens;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ClickerAuth.Application.AuthService.Commands.RenewJwt;

public class RenewJwtHandler(RefreshTokensRepository refreshTokensRepository, JwtProvider jwtProvider)
    : IRequestHandler<RenewJwtRequest, RenewJwtResponse>
{
    public async Task<RenewJwtResponse> Handle(RenewJwtRequest request, CancellationToken cancellationToken)
    {
        //TODO check existence 
        Console.WriteLine(request.RefreshToken);
        var isRevoked = await refreshTokensRepository.CheckRefreshTokenRevoked(request.RefreshToken, cancellationToken);

        Console.WriteLine(isRevoked);
        if (isRevoked)
            throw new SecurityTokenException("This token is revoked");
        
        var pair =await jwtProvider.GetNewPair(request.Username, cancellationToken);
        
        await refreshTokensRepository.RevokeRefreshToken(request.RefreshToken, cancellationToken);
        
        return new RenewJwtResponse()
        {
            RefreshToken = pair.refresh,
            AccessToken = pair.access
        };
    }
}