using Application.Abstractions;
using FluentValidation;

namespace Application.ContentTypes.Queries.GetAll;

public class GetContentTypesQueryValidator : AbstractValidator<GetContentTypesQuery>
{
	public GetContentTypesQueryValidator(IContentTypeExistenceChecker checker) =>
		RuleFor(q => q.Kind).IsInEnum();
}