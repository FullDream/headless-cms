using BuildingBlocks.Validation;
using FluentValidation;

namespace ContentTypes.Application.Create;

internal sealed class CreateContentFieldDtoValidator : AbstractValidator<CreateContentFieldDto>
{
	public CreateContentFieldDtoValidator()
	{
		RuleFor(field => field.Name).MustBeCamelCase();
		RuleFor(field => field.Label).NotEmpty();
		RuleFor(field => field.Type).IsInEnum();
	}
}