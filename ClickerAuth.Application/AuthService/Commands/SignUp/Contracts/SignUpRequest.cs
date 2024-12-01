using ClickerAuth.Application.AuthService.Commands.SignUp.Contracts;
using MediatR;

namespace ClickerAuth.Application.AuthService.Commands.SignIn.Contracts;

public record SignUpRequest(string Username, string Password) : IRequest<SignUpResponse>;