using MediatR;
using SharedKernel.Result;

namespace BuildingBlocks.Authorization;

public sealed class PermissionBehavior<TRequest, TResponse>(IPermissionChecker permissionChecker)
	: IPipelineBehavior<TRequest, TResponse> where TRequest : notnull where TResponse : IFailureFactory<TResponse>
{
	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		if (request is not IRequirePermission securedRequest)
			return await next(cancellationToken);

		var access = await permissionChecker.CheckAsync(securedRequest.Permission, cancellationToken);

		return access == PermissionAccess.None
			? TResponse.Failure(PermissionErrors.Forbidden)
			: await next(cancellationToken);
	}
}
