using ContentTypes.Application.Dtos;
using FluentValidation;

namespace ContentTypes.Application.Commands.UpdateField;

public class UpdateFieldInContentTypeCommandValidator : AbstractValidator<UpdateFieldInContentTypeCommand>
{
	public UpdateFieldInContentTypeCommandValidator()
	{
		RuleFor(c => c.ContentTypeId).NotEmpty();
		RuleFor(c => c.ContentFieldId).NotEmpty();
		RuleFor(c => c.UpdateDto).NotNull().SetValidator(new UpdateContentFieldDtoValidator());
	}
}