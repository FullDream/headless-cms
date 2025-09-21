using Application.Abstractions.Validation;
using FluentValidation;

namespace ContentTypes.Application.Commands.Update;

public class UpdateContentTypeCommandValidator : AbstractValidator<UpdateContentTypeCommand>
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