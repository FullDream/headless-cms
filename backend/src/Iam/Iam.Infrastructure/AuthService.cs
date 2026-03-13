using Iam.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Result;

namespace Iam.Infrastructure;

public class AuthService(SignInManager<IamUser> signInManager, UserManager<IamUser> userManager) : IAuthService
{
	public async Task<Result<AuthUser>> LoginAsync(string email, string password, CancellationToken cancellationToken)
	{
		var user = await userManager.FindByEmailAsync(email);

		if (user == null) return new Error("Iam.NotFound", "User not found", nameof(email), ErrorType.NotFound);

		var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

		if (!result.Succeeded)
			return new Error("Iam.IncorrectPassword",
				"The password is incorrect",
				nameof(email),
				ErrorType.Unauthenticated);

		await signInManager.SignInAsync(user, isPersistent: true);

		return new AuthUser(user.Email!);
	}

	public async Task<Result<AuthUser>> RegisterAsync(string email, string password,
		CancellationToken cancellationToken)
	{
		var existingUser = await userManager.FindByEmailAsync(email);

		if (existingUser != null)
			return new Error("Iam.UserExist", "User with this email already exists", nameof(email), ErrorType.Conflict);

		var user = new IamUser
		{
			UserName = email,
			Email = email,
			EmailConfirmed = true
		};

		var result = await userManager.CreateAsync(user, password);

		return result.Succeeded
			? new AuthUser(user.Email)
			: Result<AuthUser>.Failure(result.Errors
				.Select(e => new Error(e.Code, e.Description, null, ErrorType.BusinessRule)).ToArray());
	}

	public Task LogoutAsync(CancellationToken cancellationToken) => signInManager.SignOutAsync();
}