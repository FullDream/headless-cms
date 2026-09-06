using Iam.Domain.Roles;

namespace Iam.Application.Roles;

public sealed record RoleDto(Guid Id, string Name, RoleKind Kind, IReadOnlyCollection<RolePermissionDto> Permissions);
