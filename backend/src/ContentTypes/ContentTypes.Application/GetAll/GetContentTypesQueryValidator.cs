using FluentValidation;

namespace ContentTypes.Application.GetAll;

internal sealed class GetContentTypesQueryValidator : AbstractValidator<GetContentTypesQuery>
{
	public GetContentTypesQueryValidator() =>
		RuleFor(q => q.Kind).IsInEnum();
}