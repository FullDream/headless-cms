using Application.Abstractions.Validation;
using ContentTypes.Application.Dtos;
using FluentValidation;

namespace ContentTypes.Application.Commands.Create;

public class CreateContentTypeCommandValidator : AbstractValidator<CreateContentTypeCommand>
{
	public CreateContentTypeCommandValidator()
	{
		RuleFor(c => c.Name).MustBeKebabCase();
		RuleFor(c => c.Kind).IsInEnum();
		RuleForEach(c => c.Fields).SetValidator(new CreateContentFieldDtoValidator());
	}
}