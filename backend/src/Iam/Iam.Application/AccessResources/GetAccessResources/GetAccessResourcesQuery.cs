using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;
using Iam.Application.Roles;

namespace Iam.Application.AccessResources.GetAccessResources;

public sealed record GetAccessResourcesQuery : IQuery<IReadOnlyCollection<AccessResourceDto>>, IRequirePermission
{
	public PermissionRequirement Permission => RolePermissionRequirements.Read;
}
