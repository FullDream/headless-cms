using Iam.Domain.Roles;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.Roles.GetRoleById;

internal sealed class GetRoleByIdQueryHandler(IRoleRepository roleRepository)
	: IRequestHandler<GetRoleByIdQuery, Result<RoleDto>>
{
	public async Task<Result<RoleDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
	{
		var role = await roleRepository.FindByIdAsync(request.Id, cancellationToken);

		if (role is null) return RoleErrors.NotFound(nameof(request.Id));

		return role.ToDto();
	}
}
