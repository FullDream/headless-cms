using Application.ContentTypes.Dtos;
using FluentValidation;

namespace Application.ContentTypes.Commands.UpdateFieldInContentType;

public class UpdateFieldInContentTypeCommandValidator : AbstractValidator<UpdateFieldInContentTypeCommand>
{
	public UpdateFieldInContentTypeCommandValidator()
	{
		RuleFor(c => c.ContentTypeId).NotEmpty();
		RuleFor(c => c.ContentFieldId).NotEmpty();

		RuleFor(c => c.UpdateDto).NotNull().SetValidator(new UpdateContentFieldDtoValidator());
	}
}