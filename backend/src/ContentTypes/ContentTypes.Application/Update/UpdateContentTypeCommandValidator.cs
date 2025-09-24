using BuildingBlocks.Validation;
using FluentValidation;

namespace ContentTypes.Application.Update;

internal sealed class UpdateContentTypeCommandValidator : AbstractValidator<UpdateContentTypeCommand>
{
	public UpdateContentTypeCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty();

		RuleFor(c => c.Name)
			.MustBeKebabCase()
			.When(c => c.Name is not null);

		RuleFor(c => c.Kind).IsInEnum();
	}
}