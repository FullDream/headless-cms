using FluentValidation;

namespace ContentTypes.Application.Queries.GetAll;

public class GetContentTypesQueryValidator : AbstractValidator<GetContentTypesQuery>
{
	public GetContentTypesQueryValidator() =>
		RuleFor(q => q.Kind).IsInEnum();
}