using System.Security.Claims;
using Iam.Application.Login;
using Iam.Application.Logout;
using Iam.Application.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

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
	public async Task<IActionResult> Login(LoginRequest request)
	{
		var result = await mediator.Send(new LoginCommand(request.Email, request.Password));

		return result.IsSuccess ? NoContent() : Unauthorized();
	}

	[Authorize]
	[HttpPost("logout")]
	public async Task<IActionResult> Logout()
	{
		await mediator.Send(new LogoutCommand());

		return NoContent();
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(new RegisterCommand(request.Email, request.Password), cancellationToken);

		return result.IsSuccess ? NoContent() : Conflict();
	}

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