using FluentValidation;

namespace ContentTypes.Application.Remove;

internal sealed class RemoveContentTypeCommandValidator : AbstractValidator<RemoveContentTypeCommand>
{
	public RemoveContentTypeCommandValidator() =>
		RuleFor(c => c.Id).NotEmpty();
}