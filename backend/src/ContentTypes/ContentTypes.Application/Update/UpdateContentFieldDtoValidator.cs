using BuildingBlocks.Validation;
using FluentValidation;

namespace ContentTypes.Application.Update;

internal sealed class UpdateContentFieldDtoValidator : AbstractValidator<UpdateContentFieldDto>
{
	public UpdateContentFieldDtoValidator()
	{
		RuleFor(field => field.Name).MustBeCamelCase().When(field => field.Name is not null);
		RuleFor(field => field.Label).NotEmpty().When(field => field.Label is not null);
		RuleFor(field => field.Type).IsInEnum();
	}
}