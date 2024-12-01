using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClickerAuth.Application.AuthService.Commands.RenewJwt.Contracts;

public class RenewJwtRequest : IRequest<RenewJwtResponse>
{
    public string? RefreshToken { get; set; }
   public string? Username { get; set; }
}