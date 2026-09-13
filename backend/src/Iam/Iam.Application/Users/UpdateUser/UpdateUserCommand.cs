using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;

namespace Iam.Application.Users.UpdateUser;

public sealed record UpdateUserCommand(Guid Id, IReadOnlyCollection<Guid> RoleIds)
	: ICommand<UserDto>, IRequirePermission
{
	public PermissionRequirement Permission => UserPermissionRequirements.Update;
}
