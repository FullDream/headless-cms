using SharedKernel.Result;

namespace Iam.Application.Abstractions.Authentication;

public interface IAuthService
{
	Task<Result<AuthUser>> LoginAsync(
		string email,
		string password,
		CancellationToken cancellationToken);

	Task<Result<AuthUser>> RegisterAsync(
		string email,
		string password,
		CancellationToken cancellationToken);

	Task LogoutAsync(CancellationToken cancellationToken);
}