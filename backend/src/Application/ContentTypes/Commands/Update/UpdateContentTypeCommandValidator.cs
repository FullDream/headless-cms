using Application.Abstractions;
using Application.Common.Validation;
using FluentValidation;

namespace Application.ContentTypes.Commands.Update;

public class UpdateContentTypeCommandValidator : AbstractValidator<UpdateContentTypeCommand>
{
	public UpdateContentTypeCommandValidator(IContentTypeExistenceChecker checker)
	{
		RuleFor(c => c.Id)
			.NotEmpty()
			.MustAsync(async (id, ct) => await checker.ExistsByIdAsync(id, ct));

		RuleFor(c => c.Name)
			.MustBeKebabCase()
			.When(c => c.Name is not null);

		RuleFor(c => c.Kind).IsInEnum();
	}
}