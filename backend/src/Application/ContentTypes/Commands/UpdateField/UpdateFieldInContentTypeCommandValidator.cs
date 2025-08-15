using Application.Abstractions;
using Application.ContentTypes.Dtos;
using FluentValidation;

namespace Application.ContentTypes.Commands.UpdateField;

public class UpdateFieldInContentTypeCommandValidator : AbstractValidator<UpdateFieldInContentTypeCommand>
{
	public UpdateFieldInContentTypeCommandValidator(IContentTypeExistenceChecker checker)
	{
		RuleFor(c => c.ContentTypeId)
			.NotEmpty()
			.MustAsync(async (id, ct) => await checker.ExistsByIdAsync(id, ct));

		RuleFor(c => c.ContentFieldId).NotEmpty();

		RuleFor(c => c.UpdateDto).NotNull().SetValidator(new UpdateContentFieldDtoValidator());
	}
}