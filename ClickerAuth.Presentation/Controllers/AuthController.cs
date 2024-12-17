using ClickerAuth.Application.AuthService.Commands.RenewJwt.Contracts;
using ClickerAuth.Application.AuthService.Commands.SignIn.Contracts;
using ClickerAuth.Application.AuthService.Commands.SignUp.Contracts;
using MediatR;
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
            return new ContentResult()
            {
                Content = "Невозможно войти: " + e.Message,
                StatusCode = 404
            };
        }
        
    }

    [HttpPost(nameof(SignUp))]
    public async Task<ActionResult<SignUpResponse>> SignUp([FromBody] SignUpRequest request)
    {
        try
        {
            return await mediator.Send(request);
        }
        catch (Exception e)
        {
            return new ContentResult()
            {
                Content = "Невозможно зарегистрироваться: " + e.Message,
                StatusCode = 404
            };
        }
    }

    [HttpPost(nameof(RenewToken))]
    public async Task<ActionResult<RenewJwtResponse>> RenewToken([FromBody] RenewJwtRequest request)
    {
        try
        {
            return await mediator.Send(request);
        }
        catch (Exception e)
        {
            return new ContentResult()
            {
                Content = "Невозможно обновить токен: " + e.Message,
                StatusCode = 404
            };
        }
    }
}