using Clicker.Results;
using ClickerAuth.Application.AuthService.Commands.Auth.Contracts;
using ClickerAuth.Application.AuthService.Commands.RenewJwt.Contracts;
using ClickerAuth.Application.AuthService.Commands.SignIn.Contracts;
using ClickerAuth.Application.AuthService.Commands.SignUp.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignInRequest = ClickerAuth.Application.AuthService.Commands.Auth.Contracts.SignInRequest;
using SignInResponse = ClickerAuth.Application.AuthService.Commands.Auth.Contracts.SignInResponse;

namespace ClickerAuth.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator)
{
    [HttpPost(nameof(SignIn))]
    public async Task<ActionResult<SignInResponse>> SignIn([FromBody] SignInRequest signInRequest)
    {
        try
        {
            return await mediator.Send(signInRequest);
        }
        catch (Exception e)
        {
            return new ErrorResult("Невозможно авторизоваться: " + e.Message);
        }
        
    }

    [HttpPost(nameof(SignUp))]
    public async Task<ActionResult<SignUpResponse>> SignUp([FromBody] SignUpRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpPost(nameof(RenewToken))]
    public async Task<RenewJwtResponse> RenewToken([FromBody] RenewJwtRequest request)
    {
        Console.WriteLine(request.Username);
        return await mediator.Send(request);
    }
}