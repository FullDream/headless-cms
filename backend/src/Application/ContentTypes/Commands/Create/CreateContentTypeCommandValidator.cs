using Application.Common.Validation;
using Application.ContentTypes.Dtos;
using FluentValidation;

namespace Application.ContentTypes.Commands.Create;

public class CreateContentTypeCommandValidator : AbstractValidator<CreateContentTypeCommand>
{
	public CreateContentTypeCommandValidator()
	{
		RuleFor(c => c.Name).MustBeKebabCase();
		RuleFor(c => c.Kind).IsInEnum();
		RuleForEach(c => c.Fields).SetValidator(new CreateContentFieldDtoValidator());
	}
}