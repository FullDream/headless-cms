using Iam.Application.Abstractions.Authentication;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.Register;

public class RegisterCommandHandler(IAuthService authService)
	: IRequestHandler<RegisterCommand, Result>
{
	public Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken) =>
		authService.RegisterAsync(request.Email, request.Password, cancellationToken);
}