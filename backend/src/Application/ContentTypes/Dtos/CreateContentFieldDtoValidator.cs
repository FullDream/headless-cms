using Application.Common.Validation;
using FluentValidation;

namespace Application.ContentTypes.Dtos;

public class CreateContentFieldDtoValidator : AbstractValidator<CreateContentFieldDto>
{
	public CreateContentFieldDtoValidator()
	{
		RuleFor(field => field.Name).MustBeCamelCase();
		RuleFor(field => field.Label).NotEmpty();
		RuleFor(field => field.Type).IsInEnum();
	}
}