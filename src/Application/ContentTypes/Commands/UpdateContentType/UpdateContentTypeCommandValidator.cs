using Application.Common.Validation;
using FluentValidation;

namespace Application.ContentTypes.Commands.UpdateContentType;

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