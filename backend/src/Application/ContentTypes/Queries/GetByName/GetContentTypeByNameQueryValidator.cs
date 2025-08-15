using Application.Abstractions;
using FluentValidation;

namespace Application.ContentTypes.Queries.GetByName;

public class GetContentTypeByNameQueryValidator : AbstractValidator<GetContentTypeByNameQuery>
{
	public GetContentTypeByNameQueryValidator(IContentTypeExistenceChecker checker)
	{
		RuleFor(q => q.Name)
			.NotEmpty()
			.MustAsync(async (name, ct) => await checker.ExistsByNameAsync(name, ct));
	}
}