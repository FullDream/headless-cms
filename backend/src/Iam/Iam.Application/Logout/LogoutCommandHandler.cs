using Iam.Application.Abstractions.Authentication;
using MediatR;

namespace Iam.Application.Logout;

public class LogoutCommandHandler(IAuthService authService) : IRequestHandler<LogoutCommand>
{
	public Task Handle(LogoutCommand request, CancellationToken cancellationToken) =>
		authService.LogoutAsync(cancellationToken);
}