using FluentValidation;

namespace ContentTypes.Application.Queries.GetByName;

public class GetContentTypeByNameQueryValidator : AbstractValidator<GetContentTypeByNameQuery>
{
	public GetContentTypeByNameQueryValidator() =>
		RuleFor(q => q.Name).NotEmpty();
}