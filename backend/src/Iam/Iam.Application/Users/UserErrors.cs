using SharedKernel.Result;

namespace Iam.Application.Users;

internal static class UserErrors
{
	internal static Error NotFound(string property) =>
		new("User.NotFound", "User is not found", property, ErrorType.NotFound);
}
