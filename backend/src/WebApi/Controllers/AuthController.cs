using System.Security.Claims;
using Iam.Application.Login;
using Iam.Application.Logout;
using Iam.Application.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Results;

namespace WebApi.Controllers;

public sealed record RegisterRequest
{
	public string Email { get; init; } = default!;
	public string Password { get; init; } = default!;
}

[ApiController]
[Route("iam/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
	[HttpPost("login")]
	public async Task<OutcomeResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken) =>
		await mediator.Send(new LoginCommand(request.Email, request.Password), cancellationToken);

	[Authorize]
	[HttpPost("logout")]
	public async Task<IActionResult> Logout()
	{
		await mediator.Send(new LogoutCommand());

		return NoContent();
	}

	[HttpPost("register")]
	public async Task<OutcomeResult<RegisterResponse>> Register(RegisterRequest request,
		CancellationToken cancellationToken) =>
		await mediator.Send(new RegisterCommand(request.Email, request.Password), cancellationToken);

	[Authorize]
	[HttpGet("me")]
	public IActionResult Me()
	{
		return Ok(new
		{
			id = User.FindFirstValue(ClaimTypes.NameIdentifier),
			email = User.FindFirstValue(ClaimTypes.Email),
			permissions = User.FindAll("permission").Select(x => x.Value)
		});
	}
}