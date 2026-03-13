using Iam.Application.Abstractions.Authentication;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.Login;

public class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, Result>
{
	public Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken) =>
		authService.LoginAsync(request.Username, request.Password, cancellationToken);
}