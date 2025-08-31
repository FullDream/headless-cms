using FluentValidation;

namespace Application.ContentTypes.Commands.Remove;

public class RemoveContentTypeCommandValidator : AbstractValidator<RemoveContentTypeCommand>
{
	public RemoveContentTypeCommandValidator() =>
		RuleFor(c => c.Id).NotEmpty();
}