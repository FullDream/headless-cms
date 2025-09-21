using FluentValidation;

namespace ContentTypes.Application.Commands.RemoveField;

public class RemoveFieldFromContentTypeCommandValidator : AbstractValidator<RemoveFieldFromContentTypeCommand>
{
	public RemoveFieldFromContentTypeCommandValidator()
	{
		RuleFor(c => c.ContentTypeId).NotEmpty();
		RuleFor(c => c.ContentFieldId).NotEmpty();
	}
}