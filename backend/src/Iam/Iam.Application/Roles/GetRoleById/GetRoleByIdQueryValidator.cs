using FluentValidation;

namespace Iam.Application.Roles.GetRoleById;

internal sealed class GetRoleByIdQueryValidator : AbstractValidator<GetRoleByIdQuery>
{
	public GetRoleByIdQueryValidator() => RuleFor(q => q.Id).NotEmpty();
}
