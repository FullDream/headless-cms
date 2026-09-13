using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;

namespace Iam.Application.Users.GetUsers;

public sealed record GetUsersQuery : IQuery<IReadOnlyCollection<UserDto>>, IRequirePermission
{
	public PermissionRequirement Permission => UserPermissionRequirements.Read;
}
