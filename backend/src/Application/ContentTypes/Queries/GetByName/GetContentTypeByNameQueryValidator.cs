using FluentValidation;

namespace Application.ContentTypes.Queries.GetByName;

public class GetContentTypeByNameQueryValidator : AbstractValidator<GetContentTypeByNameQuery>
{
	public GetContentTypeByNameQueryValidator() =>
		RuleFor(q => q.Name).NotEmpty();
}