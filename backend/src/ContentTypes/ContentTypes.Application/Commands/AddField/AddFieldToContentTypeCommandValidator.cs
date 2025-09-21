using ContentTypes.Application.Dtos;
using FluentValidation;

namespace ContentTypes.Application.Commands.AddField;

public class AddFieldToContentTypeCommandValidator : AbstractValidator<AddFieldToContentTypeCommand>
{
	public AddFieldToContentTypeCommandValidator()
	{
		RuleFor(command => command.ContentTypeId).NotEmpty();
		RuleFor(command => command.Field).SetValidator(new CreateContentFieldDtoValidator());
	}
}