using FluentValidation;

namespace Application.ContentTypes.Commands.RemoveContentType;

public class RemoveContentTypeCommandValidator : AbstractValidator<RemoveContentTypeCommand>
{
	public RemoveContentTypeCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty();
	}
}