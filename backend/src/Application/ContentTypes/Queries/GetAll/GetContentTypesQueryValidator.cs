using FluentValidation;

namespace Application.ContentTypes.Queries.GetAll;

public class GetContentTypesQueryValidator : AbstractValidator<GetContentTypesQuery>
{
	public GetContentTypesQueryValidator() =>
		RuleFor(q => q.Kind).IsInEnum();
}