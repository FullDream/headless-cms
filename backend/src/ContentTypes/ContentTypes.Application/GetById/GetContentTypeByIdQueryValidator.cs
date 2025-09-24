using FluentValidation;

namespace ContentTypes.Application.GetById;

internal sealed class GetContentTypeByIdQueryValidator : AbstractValidator<GetContentTypeByIdQuery>
{
	public GetContentTypeByIdQueryValidator() =>
		RuleFor(q => q.Id).NotEmpty();
}