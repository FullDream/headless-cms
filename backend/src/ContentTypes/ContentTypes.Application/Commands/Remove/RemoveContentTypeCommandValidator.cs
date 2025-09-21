using FluentValidation;

namespace ContentTypes.Application.Commands.Remove;

public class RemoveContentTypeCommandValidator : AbstractValidator<RemoveContentTypeCommand>
{
	public RemoveContentTypeCommandValidator() =>
		RuleFor(c => c.Id).NotEmpty();
}