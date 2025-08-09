using FluentValidation;

namespace Application.ContentTypes.Commands.RemoveField;

public class RemoveFieldFromContentTypeCommandValidator : AbstractValidator<RemoveFieldFromContentTypeCommand>
{
	public RemoveFieldFromContentTypeCommandValidator()
	{
		RuleFor(c => c.ContentTypeId).NotEmpty();
		RuleFor(c => c.ContentFieldId).NotEmpty();
	}
}