using SharedKernel.Result;

namespace Iam.Domain.Roles;

public static class RoleErrors
{
	public static readonly Error AlreadyExists = new(
		"Role.AlreadyExists",
		"A role with this name already exists.",
		Type: ErrorType.Conflict);

	public static readonly Error NameRequired = new(
		"Role.NameRequired",
		"Role name is required.",
		Type: ErrorType.Validation);

	public static readonly Error SuperAdminCannotBeRenamed = new(
		"Role.SuperAdminCannotBeRenamed",
		"SuperAdmin role cannot be renamed.",
		Type: ErrorType.BusinessRule);

	public static readonly Error SuperAdminPermissionsCannotBeModified = new(
		"Role.SuperAdminPermissionsCannotBeModified",
		"SuperAdmin permissions cannot be modified.",
		Type: ErrorType.BusinessRule);

	public static readonly Error SuperAdminCannotBeRemoved = new(
		"Role.SuperAdminCannotBeRemoved",
		"SuperAdmin role cannot be removed.",
		Type: ErrorType.BusinessRule);

	public static Error NotFound(string property) =>
		new("Role.NotFound", "Role is not found", property, ErrorType.NotFound);
}
