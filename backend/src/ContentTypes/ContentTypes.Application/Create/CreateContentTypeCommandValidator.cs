using BuildingBlocks.Validation;
using ContentTypes.Application.AddField;
using FluentValidation;

namespace ContentTypes.Application.Create;

internal sealed class CreateContentTypeCommandValidator : AbstractValidator<CreateContentTypeCommand>
{
	public CreateContentTypeCommandValidator()
	{
		RuleFor(c => c.Name).MustBeKebabCase();
		RuleFor(c => c.Kind).IsInEnum();
		RuleForEach(c => c.Fields).SetValidator(new CreateContentFieldDtoValidator());
	}
}