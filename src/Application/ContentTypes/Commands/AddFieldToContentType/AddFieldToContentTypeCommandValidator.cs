using Application.ContentTypes.Dtos;
using FluentValidation;

namespace Application.ContentTypes.Commands.AddFieldToContentType;

public class AddFieldToContentTypeCommandValidator : AbstractValidator<AddFieldToContentTypeCommand>
{
	public AddFieldToContentTypeCommandValidator()
	{
		RuleFor(command => command.ContentTypeId).NotEmpty();
		RuleFor(command => command.Field).SetValidator(new CreateContentFieldDtoValidator());
	}
}