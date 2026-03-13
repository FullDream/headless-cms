using Iam.Application.Abstractions.Authentication;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.Register;

public class RegisterCommandHandler(IAuthService authService)
	: IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
	public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
	{
		var result = await authService.RegisterAsync(request.Email, request.Password, cancellationToken);

		return result.IsSuccess ? new RegisterResponse() : result.Errors;
	}
}