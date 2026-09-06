using FluentValidation;

namespace Iam.Application.Roles.RemoveRole;

internal sealed class RemoveRoleCommandValidator : AbstractValidator<RemoveRoleCommand>
{
	public RemoveRoleCommandValidator() => RuleFor(x => x.Id).NotEmpty();
}
