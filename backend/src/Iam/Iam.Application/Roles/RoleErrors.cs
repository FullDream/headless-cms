using SharedKernel.Result;

namespace Iam.Application.Roles;

internal static class RoleErrors
{
	internal static readonly Error AlreadyExists = new(
		"Role.AlreadyExists",
		"A role with this name already exists.",
		Type: ErrorType.Conflict);

	internal static readonly Error SuperAdminCannotBeRemoved = new(
		"Role.SuperAdminCannotBeRemoved",
		"SuperAdmin role cannot be removed.",
		Type: ErrorType.BusinessRule);

	internal static Error NotFound(string property) =>
		new("Role.NotFound", "Role is not found", property, ErrorType.NotFound);
}
