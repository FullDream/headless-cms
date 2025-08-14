using Application.Abstractions;
using Application.Common.Validation;
using Application.ContentTypes.Dtos;
using FluentValidation;

namespace Application.ContentTypes.Commands.Create;

public class CreateContentTypeCommandValidator : AbstractValidator<CreateContentTypeCommand>
{
	public CreateContentTypeCommandValidator(IContentTypeExistenceChecker checker)
	{
		RuleFor(c => c.Name)
			.MustBeKebabCase()
			.MustAsync(async (name, ct) => !await checker.ExistsByNameAsync(name!, ct));

		RuleFor(c => c.Kind).IsInEnum();

		RuleForEach(c => c.Fields).SetValidator(new CreateContentFieldDtoValidator());
	}
}