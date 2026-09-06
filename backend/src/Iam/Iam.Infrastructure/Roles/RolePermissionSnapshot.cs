using Iam.Domain.Roles;

namespace Iam.Infrastructure.Roles;

internal sealed record RolePermissionSnapshot(Guid Id, RoleKind Kind, IReadOnlyCollection<Permission> Permissions);
