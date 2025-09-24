using ContentTypes.Application.Update;
using FluentValidation;

namespace ContentTypes.Application.UpdateField;

internal sealed class UpdateFieldInContentTypeCommandValidator : AbstractValidator<UpdateFieldInContentTypeCommand>
{
	public UpdateFieldInContentTypeCommandValidator()
	{
		RuleFor(c => c.ContentTypeId).NotEmpty();
		RuleFor(c => c.ContentFieldId).NotEmpty();
		RuleFor(c => c.UpdateDto).NotNull().SetValidator(new UpdateContentFieldDtoValidator());
	}
}