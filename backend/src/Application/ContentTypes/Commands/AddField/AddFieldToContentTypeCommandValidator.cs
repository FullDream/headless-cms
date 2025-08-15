using Application.Abstractions;
using Application.ContentTypes.Dtos;
using FluentValidation;

namespace Application.ContentTypes.Commands.AddField;

public class AddFieldToContentTypeCommandValidator : AbstractValidator<AddFieldToContentTypeCommand>
{
	public AddFieldToContentTypeCommandValidator(IContentTypeExistenceChecker checker)
	{
		RuleFor(command => command.ContentTypeId)
			.NotEmpty()
			.MustAsync(async (id, ct) => await checker.ExistsByIdAsync(id, ct));

		RuleFor(command => command.Field).SetValidator(new CreateContentFieldDtoValidator());
	}
}