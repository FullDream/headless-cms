using SharedKernel.Result;

namespace Iam.Domain.Roles;

public sealed class Role
{
	private readonly List<Permission> permissions = [];

	private Role(Guid id, string name, RoleKind kind)
	{
		Id = id;
		Name = name;
		Kind = kind;
	}

	public Guid Id { get; }

	public string Name { get; private set; }

	public RoleKind Kind { get; }

	public IReadOnlyCollection<Permission> Permissions => permissions;

	public static Role Restore(Guid id, string name, RoleKind kind, IEnumerable<Permission> permissions)
	{
		var role = new Role(id, name, kind);

		role.permissions.AddRange(permissions);

		return role;
	}

	public static Result<Role> Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			return RoleErrors.NameRequired;

		return new Role(Guid.NewGuid(), name.Trim(), RoleKind.Custom);
	}

	public static Role CreateSuperAdmin(Guid id) => new(id, "SuperAdmin", RoleKind.SuperAdmin);

	public Result Rename(string name)
	{
		if (Kind == RoleKind.SuperAdmin)
			return RoleErrors.SuperAdminCannotBeRenamed;

		if (string.IsNullOrWhiteSpace(name))
			return RoleErrors.NameRequired;

		Name = name.Trim();

		return Result.Success();
	}

	public Result SetPermission(Permission permission)
	{
		if (Kind == RoleKind.SuperAdmin)
			return RoleErrors.SuperAdminPermissionsCannotBeModified;

		permissions.RemoveAll(x => x.ResourceId == permission.ResourceId && x.Action == permission.Action);

		permissions.Add(permission);

		return Result.Success();
	}

	public Result RemovePermission(Guid resourceId, PermissionAction action)
	{
		if (Kind == RoleKind.SuperAdmin)
			return RoleErrors.SuperAdminPermissionsCannotBeModified;

		permissions.RemoveAll(x => x.ResourceId == resourceId && x.Action == action);

		return Result.Success();
	}
}
