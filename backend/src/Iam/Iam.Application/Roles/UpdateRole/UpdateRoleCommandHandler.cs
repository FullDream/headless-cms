using Iam.Domain.Roles;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.Roles.UpdateRole;

internal sealed class UpdateRoleCommandHandler(IRoleRepository roleRepository)
	: IRequestHandler<UpdateRoleCommand, Result<RoleDto>>
{
	public async Task<Result<RoleDto>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
	{
		var role = await roleRepository.FindByIdAsync(request.Id, cancellationToken);

		if (role is null) return RoleErrors.NotFound(nameof(request.Id));

		var renameResult = role.Rename(request.Name);

		if (renameResult.IsFailure) return renameResult.Errors;

		var duplicate = await roleRepository.FindByNameAsync(role.Name, cancellationToken);

		if (duplicate is not null && duplicate.Id != role.Id) return RoleErrors.AlreadyExists;

		foreach (var permission in role.Permissions.ToArray())
		{
			var removeResult = role.RemovePermission(permission.ResourceId, permission.Action);

			if (removeResult.IsFailure) return removeResult.Errors;
		}

		foreach (var permission in request.Permissions)
		{
			var permissionResult = role.SetPermission(
				new Permission(permission.ResourceId, permission.Action, permission.Scope));

			if (permissionResult.IsFailure) return permissionResult.Errors;
		}

		await roleRepository.UpdateAsync(role, cancellationToken);

		return role.ToDto();
	}
}
