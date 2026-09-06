namespace Iam.Domain.Roles;

public sealed record Permission(Guid ResourceId, PermissionAction Action, AccessScope Scope);
