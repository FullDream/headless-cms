using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace WebApi.Common.OpenApi;

// TODO: Refactor this
public class FluentValidationSchemaTransformer : IOpenApiSchemaTransformer
{
	public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context,
		CancellationToken cancellationToken)
	{
		var applicationServices = context.ApplicationServices;
		var targetType = context.JsonTypeInfo.Type;
		var validatorType = typeof(IValidator<>).MakeGenericType(targetType);

		if (applicationServices.GetService(validatorType) is not IValidator validator)
			return Task.CompletedTask;


		var descriptor = validator.CreateDescriptor();

		if (schema.Type == "object" && schema.Properties is { Count: > 0 })
		{
			foreach (var (key, value) in schema.Properties)
			{
				if (schema.Properties.TryGetValue(key, out var local))
				{
					var jsonProp = context.JsonTypeInfo.Properties.FirstOrDefault(p => p.Name == key);

					var clrName = ResolveClrMemberName(targetType, key);

					if (string.IsNullOrEmpty(clrName)) continue;

					var rules = descriptor.GetRulesForMember(clrName);

					if (rules is null) continue;

					foreach (var rule in rules)
					{
						foreach (var component in rule.Components)
						{
							var propertyValidator = component.Validator; // IPropertyValidator
							MapValidatorToSchema(propertyValidator, value, schema, key);
						}
					}
				}
			}

			Console.WriteLine(schema.Title);
		}


		return Task.CompletedTask;
	}


	private static string? ResolveClrMemberName(Type type, string jsonName)
	{
		const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;

		foreach (var info in type.GetProperties(bindingFlags))
		{
			var attr = info.GetCustomAttribute<JsonPropertyNameAttribute>();
			if (attr is not null && string.Equals(attr.Name, jsonName, StringComparison.Ordinal))
				return info.Name;
		}

		var camel = JsonNamingPolicy.CamelCase;
		foreach (var info in type.GetProperties(bindingFlags))
		{
			if (string.Equals(camel.ConvertName(info.Name), jsonName, StringComparison.Ordinal))
				return info.Name;
		}

		var direct = type.GetProperty(jsonName, bindingFlags | BindingFlags.IgnoreCase);
		return direct?.Name;
	}


	private static void MapValidatorToSchema(IPropertyValidator rule, OpenApiSchema prop, OpenApiSchema parent,
		string propName)
	{
		// NotEmpty / NotNull -> required + minLength:1 (для строк)
		if (rule is INotNullValidator)
		{
			parent.Required ??= new HashSet<string>();
			parent.Required.Add(propName);
			prop.Nullable = false; // OpenAPI 3.0
		}

		// NotEmpty -> required + minLength/minItems = 1
		if (rule is INotEmptyValidator)
		{
			parent.Required ??= new HashSet<string>();
			parent.Required.Add(propName);

			if (prop.Type == "string")
				prop.MinLength = Math.Max(prop.MinLength ?? 0, 1);

			if (prop.Type == "array")
				prop.MinItems = Math.Max(prop.MinItems ?? 0, 1);
		}

		// Length(min,max)
		if (rule is ILengthValidator len)
		{
			if (len.Min > 0) prop.MinLength = Math.Max(prop.MinLength ?? 0, len.Min);
			if (len.Max > 0) prop.MaxLength = prop.MaxLength is { } cur ? Math.Min(cur, len.Max) : len.Max;
		}

		// Matches(regex)
		if (rule is IRegularExpressionValidator rx && !string.IsNullOrWhiteSpace(rx.Expression))
		{
			prop.Pattern = rx.Expression;
		}

		// InclusiveBetween/ExclusiveBetween для чисел/дат
		if (rule.GetType().Name.Contains("InclusiveBetweenValidator"))
		{
			var (min, max) = GetRange(rule);
			if (min is not null)
			{
				prop.Minimum = TryToDecimal(min);
				prop.ExclusiveMinimum = false;
			}

			if (max is not null)
			{
				prop.Maximum = TryToDecimal(max);
				prop.ExclusiveMaximum = false;
			}
		}

		if (rule.GetType().Name.Contains("ExclusiveBetweenValidator"))
		{
			var (min, max) = GetRange(rule);
			if (min is not null)
			{
				prop.Minimum = TryToDecimal(min);
				prop.ExclusiveMinimum = true;
			}

			if (max is not null)
			{
				prop.Maximum = TryToDecimal(max);
				prop.ExclusiveMaximum = true;
			}
		}

		// EmailAddress -> format: email
		if (rule.GetType().Name.Contains("EmailValidator") && prop.Type == "string")
		{
			prop.Format = "email";
		}
	}

	private static (object? min, object? max) GetRange(IPropertyValidator validator)
	{
		var t = validator.GetType();
		var from = t.GetProperty("From")?.GetValue(validator);
		var to = t.GetProperty("To")?.GetValue(validator);
		return (from, to);
	}

	private static decimal? TryToDecimal(object? value)
	{
		if (value is null) return null;
		try
		{
			return Convert.ToDecimal(value);
		}
		catch
		{
			return null;
		}
	}
}