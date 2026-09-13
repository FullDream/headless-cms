using System.Security.Claims;
using BuildingBlocks.Presentation.Results;
using Iam.Application.Authentication.Login;
using Iam.Application.Authentication.Logout;
using Iam.Application.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RegisterRequest = Iam.Presentation.Contracts.Authentication.RegisterRequest;

namespace Iam.Presentation.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
	[HttpPost("login")]
	public async Task<OutcomeResult> Login(LoginRequest request, CancellationToken cancellationToken) =>
		await mediator.Send(new LoginCommand(request.Email, request.Password), cancellationToken);

	[Authorize]
	[HttpPost("logout")]
	public async Task<OutcomeResult> Logout(CancellationToken cancellationToken) =>
		await mediator.Send(new LogoutCommand(), cancellationToken);

	[HttpPost("register")]
	public async Task<OutcomeResult> Register(RegisterRequest request, CancellationToken cancellationToken) =>
		await mediator.Send(new RegisterCommand(request.Email, request.Password), cancellationToken);

	[Authorize]
	[HttpGet("me")]
	public IActionResult Me()
	{
		return Ok(
			new
			{
				id = User.FindFirstValue(ClaimTypes.NameIdentifier),
				email = User.FindFirstValue(ClaimTypes.Email),
				permissions = User.FindAll("permission").Select(x => x.Value)
			});
	}
}
