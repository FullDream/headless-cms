using FluentValidation;
using Iam.Domain.AccessResource;

namespace Iam.Application.Roles;

internal sealed class RolePermissionDtoValidator : AbstractValidator<RolePermissionDto>
{
	public RolePermissionDtoValidator(IAccessResourceRepository resourceRepository)
	{
		RuleFor(x => x.ResourceId).NotEmpty();
		RuleFor(x => x.Action).IsInEnum();
		RuleFor(x => x.Scope).IsInEnum();
		RuleFor(x => x)
			.MustAsync(async (permission, cancellationToken) =>
			{
				var resource = await resourceRepository.FindByIdAsync(permission.ResourceId, cancellationToken);
				return resource is not null && resource.Supports(permission.Action, permission.Scope);
			})
			.WithMessage("The resource must support the requested action and scope.");
	}
}
