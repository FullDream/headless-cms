using Application.Abstractions;
using FluentValidation;

namespace Application.ContentTypes.Commands.Remove;

public class RemoveContentTypeCommandValidator : AbstractValidator<RemoveContentTypeCommand>
{
	public RemoveContentTypeCommandValidator(IContentTypeExistenceChecker checker)
	{
		RuleFor(c => c.Id)
			.NotEmpty()
			.MustAsync(async (id, ct) => await checker.ExistsByIdAsync(id, ct));
	}
}