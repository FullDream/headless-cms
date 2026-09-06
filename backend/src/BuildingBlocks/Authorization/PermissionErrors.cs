using SharedKernel.Result;

namespace BuildingBlocks.Authorization;

public class PermissionErrors
{
	public static readonly Error Forbidden = new(
		"Authorization.Forbidden",
		"You do not have permission to perform this action.",
		Type: ErrorType.Forbidden);
}
