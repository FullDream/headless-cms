using Iam.Application.Roles;

namespace Iam.Presentation.Contracts.Roles;

public sealed record UpdateRoleDto(string Name, IReadOnlyCollection<RolePermissionDto> Permissions);
