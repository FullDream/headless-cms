using FluentValidation;

namespace ContentTypes.Application.Queries.GetById;

public class GetContentTypeByIdQueryValidator : AbstractValidator<GetContentTypeByIdQuery>
{
	public GetContentTypeByIdQueryValidator() =>
		RuleFor(q => q.Id).NotEmpty();
}