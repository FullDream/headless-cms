using FluentValidation;

namespace Iam.Application.Users.UpdateUser;

internal sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.RoleIds)
			.NotNull()
			.Must(roleIds => roleIds is null || roleIds.Distinct().Count() == roleIds.Count)
			.WithMessage("Role IDs must be unique.");
		RuleForEach(x => x.RoleIds).NotEmpty();
	}
}
