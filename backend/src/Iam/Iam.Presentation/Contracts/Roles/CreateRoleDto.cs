using Iam.Application.Roles;

namespace Iam.Presentation.Contracts.Roles;

public sealed record CreateRoleDto(string Name, IReadOnlyCollection<RolePermissionDto> Permissions);
