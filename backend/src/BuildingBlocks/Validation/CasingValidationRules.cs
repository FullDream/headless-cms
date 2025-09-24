using FluentValidation;

namespace BuildingBlocks.Validation;

public static class CasingValidationRules
{
	public static IRuleBuilderOptions<T, string?> MustBeKebabCase<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
		ruleBuilder
			.NotEmpty()
			.Matches("^[a-z]+(-[a-z0-9]+)*$")
			.WithMessage("Value must be in kebab-case (lowercase words separated by hyphens).");

	public static IRuleBuilderOptions<T, string?> MustBeCamelCase<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
		ruleBuilder
			.NotEmpty()
			.Matches("^[a-z][a-zA-Z0-9]*$")
			.WithMessage("Value must be in camelCase (start with lowercase, no symbols).");
}