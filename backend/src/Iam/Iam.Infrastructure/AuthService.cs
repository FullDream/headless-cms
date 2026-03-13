using Iam.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Result;

namespace Iam.Infrastructure;

public class AuthService(SignInManager<IamUser> signInManager, UserManager<IamUser> userManager) : IAuthService
{
	private static readonly Error InvalidCredentialsError =
		new("Iam.InvalidCredentials", "Invalid email or password", null, ErrorType.Unauthenticated);

	private static readonly Error RegistrationFailedError =
		new("Iam.RegistrationFailed", "Registration failed", null, ErrorType.Conflict);

	public async Task<Result> LoginAsync(string email, string password, CancellationToken cancellationToken)
	{
		var user = await userManager.FindByEmailAsync(email);

		if (user == null) return InvalidCredentialsError;

		var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

		if (!result.Succeeded) return InvalidCredentialsError;

		await signInManager.SignInAsync(user, isPersistent: true);

		return Result.Success();
	}

	public async Task<Result> RegisterAsync(string email, string password,
		CancellationToken cancellationToken)
	{
		var existingUser = await userManager.FindByEmailAsync(email);

		if (existingUser != null) return RegistrationFailedError;

		var user = new IamUser
		{
			UserName = email,
			Email = email,
			EmailConfirmed = true
		};

		var result = await userManager.CreateAsync(user, password);

		return result.Succeeded ? Result.Success() : RegistrationFailedError;
	}

	public Task LogoutAsync(CancellationToken cancellationToken) => signInManager.SignOutAsync();
}