using Iam.Application.Roles;

namespace WebApi.Contracts.Roles;

public sealed record UpdateRoleDto(string Name, IReadOnlyCollection<RolePermissionDto> Permissions);
