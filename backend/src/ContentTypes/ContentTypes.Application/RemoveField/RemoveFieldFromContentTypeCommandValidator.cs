using FluentValidation;

namespace ContentTypes.Application.RemoveField;

internal sealed class RemoveFieldFromContentTypeCommandValidator : AbstractValidator<RemoveFieldFromContentTypeCommand>
{
	public RemoveFieldFromContentTypeCommandValidator()
	{
		RuleFor(c => c.ContentTypeId).NotEmpty();
		RuleFor(c => c.ContentFieldId).NotEmpty();
	}
}