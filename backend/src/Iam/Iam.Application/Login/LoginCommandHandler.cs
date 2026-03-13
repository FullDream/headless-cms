using Iam.Application.Abstractions.Authentication;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.Login;

public class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
	public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		var result = await authService.LoginAsync(request.Username, request.Password, cancellationToken);


		return result.IsSuccess ? new LoginResponse() : result.Errors;
	}
}