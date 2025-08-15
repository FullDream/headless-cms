using Application.Abstractions;
using FluentValidation;

namespace Application.ContentTypes.Queries.GetById;

public class GetContentTypeByIdQueryValidator : AbstractValidator<GetContentTypeByIdQuery>
{
	public GetContentTypeByIdQueryValidator(IContentTypeExistenceChecker checker)
	{
		RuleFor(q => q.Id)
			.NotEmpty()
			.MustAsync(async (id, ct) => await checker.ExistsByIdAsync(id, ct));
	}
}