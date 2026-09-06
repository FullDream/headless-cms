using Iam.Application.Roles;

namespace WebApi.Contracts.Roles;

public sealed record CreateRoleDto(string Name, IReadOnlyCollection<RolePermissionDto> Permissions);
