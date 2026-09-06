using Iam.Domain;

namespace Iam.Infrastructure.Roles;

internal sealed class PersistencePermission
{
	public Guid RoleId { get; set; }

	public Guid ResourceId { get; set; }

	public PermissionAction Action { get; set; }

	public AccessScope Scope { get; set; }
}
