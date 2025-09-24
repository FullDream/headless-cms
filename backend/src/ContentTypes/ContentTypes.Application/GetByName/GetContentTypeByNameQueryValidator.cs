using FluentValidation;

namespace ContentTypes.Application.GetByName;

internal sealed class GetContentTypeByNameQueryValidator : AbstractValidator<GetContentTypeByNameQuery>
{
	public GetContentTypeByNameQueryValidator() =>
		RuleFor(q => q.Name).NotEmpty();
}