using Iam.Domain.Roles;
using Microsoft.AspNetCore.Identity;

namespace Iam.Infrastructure.Roles;

internal sealed class PersistenceRole : IdentityRole<Guid>
{
	internal RoleKind Kind { get; set; }

	internal ICollection<PersistencePermission> Permissions { get; private set; } = [];
}
