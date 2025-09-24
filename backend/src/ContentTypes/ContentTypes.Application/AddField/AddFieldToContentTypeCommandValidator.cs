using ContentTypes.Application.Create;
using FluentValidation;

namespace ContentTypes.Application.AddField;

internal sealed class AddFieldToContentTypeCommandValidator : AbstractValidator<AddFieldToContentTypeCommand>
{
	public AddFieldToContentTypeCommandValidator()
	{
		RuleFor(command => command.ContentTypeId).NotEmpty();
		RuleFor(command => command.Field).SetValidator(new CreateContentFieldDtoValidator());
	}
}