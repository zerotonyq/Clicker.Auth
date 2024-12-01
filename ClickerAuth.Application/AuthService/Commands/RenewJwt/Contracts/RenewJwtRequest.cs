using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClickerAuth.Application.AuthService.Commands.RenewJwt.Contracts;

public class RenewJwtRequest : IRequest<RenewJwtResponse>
{
    public required string RefreshToken { get; set; }
   public required string Username { get; set; }
}