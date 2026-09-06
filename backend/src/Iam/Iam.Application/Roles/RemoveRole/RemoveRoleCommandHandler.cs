using Iam.Domain.Roles;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.Roles.RemoveRole;

internal sealed class RemoveRoleCommandHandler(IRoleRepository roleRepository)
	: IRequestHandler<RemoveRoleCommand, Result<RoleDto>>
{
	public async Task<Result<RoleDto>> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
	{
		var role = await roleRepository.FindByIdAsync(request.Id, cancellationToken);

		if (role is null) return RoleErrors.NotFound(nameof(request.Id));

		if (role.Kind == RoleKind.SuperAdmin) return RoleErrors.SuperAdminCannotBeRemoved;

		await roleRepository.RemoveAsync(role, cancellationToken);

		return role.ToDto();
	}
}
