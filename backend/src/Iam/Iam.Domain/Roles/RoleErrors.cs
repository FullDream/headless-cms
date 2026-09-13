using SharedKernel.Result;

namespace Iam.Domain.Roles;

internal static class RoleErrors
{
	internal static readonly Error NameRequired = new(
		"Role.NameRequired",
		"Role name is required.",
		Type: ErrorType.Validation);

	internal static readonly Error SuperAdminCannotBeRenamed = new(
		"Role.SuperAdminCannotBeRenamed",
		"SuperAdmin role cannot be renamed.",
		Type: ErrorType.BusinessRule);

	internal static readonly Error SuperAdminPermissionsCannotBeModified = new(
		"Role.SuperAdminPermissionsCannotBeModified",
		"SuperAdmin permissions cannot be modified.",
		Type: ErrorType.BusinessRule);
}
