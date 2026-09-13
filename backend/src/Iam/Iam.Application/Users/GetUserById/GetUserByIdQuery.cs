using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;

namespace Iam.Application.Users.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<UserDto>, IRequirePermission
{
	public PermissionRequirement Permission => UserPermissionRequirements.Read;
}
