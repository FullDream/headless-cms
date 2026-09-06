using Iam.Domain.Roles;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.Roles.CreateRole;

internal sealed class CreateRoleCommandHandler(IRoleRepository roleRepository)
	: IRequestHandler<CreateRoleCommand, Result<RoleDto>>
{
	public async Task<Result<RoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
	{
		var result = Role.Create(request.Name);

		if (result.IsFailure) return result.Errors;

		var role = result.Value;
		var duplicate = await roleRepository.FindByNameAsync(role.Name, cancellationToken);

		if (duplicate is not null) return RoleErrors.AlreadyExists;

		foreach (var permission in request.Permissions)
		{
			var permissionResult = role.SetPermission(
				new Permission(permission.ResourceId, permission.Action, permission.Scope));

			if (permissionResult.IsFailure) return permissionResult.Errors;
		}

		await roleRepository.AddAsync(role, cancellationToken);

		return role.ToDto();
	}
}
