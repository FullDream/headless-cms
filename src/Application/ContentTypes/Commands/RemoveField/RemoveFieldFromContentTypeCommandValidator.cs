using Application.Abstractions;
using FluentValidation;

namespace Application.ContentTypes.Commands.RemoveField;

public class RemoveFieldFromContentTypeCommandValidator : AbstractValidator<RemoveFieldFromContentTypeCommand>
{
	public RemoveFieldFromContentTypeCommandValidator(IContentTypeExistenceChecker checker)
	{
		RuleFor(c => c.ContentTypeId)
			.NotEmpty()
			.MustAsync(async (id, ct) => await checker.ExistsByIdAsync(id, ct));

		RuleFor(c => c.ContentFieldId).NotEmpty();
	}
}