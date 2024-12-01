using MediatR;

namespace ClickerAuth.Application.AuthService.Commands.Auth.Contracts;

public record SignInRequest(string Username, string Password) : IRequest<SignInResponse>;