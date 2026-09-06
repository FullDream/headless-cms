using Iam.Domain.Roles;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.Roles.GetRoles;

internal sealed class GetRolesQueryHandler(IRoleRepository roleRepository)
	: IRequestHandler<GetRolesQuery, Result<IReadOnlyCollection<RoleDto>>>
{
	public async Task<Result<IReadOnlyCollection<RoleDto>>> Handle(
		GetRolesQuery request,
		CancellationToken cancellationToken)
	{
		var roles = await roleRepository.FindManyAsync(cancellationToken);

		return roles.Select(role => role.ToDto()).ToArray();
	}
}
