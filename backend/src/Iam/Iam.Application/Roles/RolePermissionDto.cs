using Iam.Domain;

namespace Iam.Application.Roles;

public sealed record RolePermissionDto(Guid ResourceId, PermissionAction Action, AccessScope Scope);
