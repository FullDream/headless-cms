using Iam.Application.Abstractions.Authentication;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.Logout;

public class LogoutCommandHandler(IAuthService authService) : IRequestHandler<LogoutCommand, Result>
{
	public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
	{
		await authService.LogoutAsync(cancellationToken);

		return Result.Success();
	}
}