using FluentValidation;

namespace Iam.Application.Users.GetUserById;

internal sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
	public GetUserByIdQueryValidator() => RuleFor(x => x.Id).NotEmpty();
}
