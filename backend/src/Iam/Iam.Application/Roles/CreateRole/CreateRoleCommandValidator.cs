using FluentValidation;

namespace Iam.Application.Roles.CreateRole;

internal sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
	public CreateRoleCommandValidator(RolePermissionDtoValidator permissionValidator)
	{
		RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
		RuleFor(x => x.Permissions)
			.NotNull()
			.Must(permissions => permissions is null ||
			                     permissions.Select(x => (x.ResourceId, x.Action)).Distinct().Count() ==
			                     permissions.Count)
			.WithMessage("Permissions must have unique resource and action pairs.");
		RuleForEach(x => x.Permissions).SetValidator(permissionValidator);
	}
}
