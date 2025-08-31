using FluentValidation;

namespace Application.ContentTypes.Queries.GetById;

public class GetContentTypeByIdQueryValidator : AbstractValidator<GetContentTypeByIdQuery>
{
	public GetContentTypeByIdQueryValidator() =>
		RuleFor(q => q.Id).NotEmpty();
}