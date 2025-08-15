using Application.Abstractions;
using Application.Common.Validation;
using FluentValidation;

namespace Application.ContentEntries.Commands.Create;

public class CreateContentEntryCommandValidator : AbstractValidator<CreateContentEntryCommand>
{
	public CreateContentEntryCommandValidator(IContentTypeFieldsProvider provider, IContentTypeExistenceChecker checker)
	{
		RuleFor(command => command.ContentTypeName).MustBeKebabCase()
			.MustAsync(async (name, ct) => await checker.ExistsByNameAsync(name!, ct));


		RuleFor(command => command).CustomAsync(async (command, context, ct) =>
		{
			var snap = await provider.FindByNameAsync(command.ContentTypeName, ct);

			if (snap is null)
			{
				context.AddFailure("contentTypeName", "Internal: conversion did not run.");
				return;
			}

			var required = snap.Fields.Values
				.Where(f => f.IsRequired)
				.Select(f => f.Name);


			var missing = required
				.Except(command.ConvertedFields.Keys, StringComparer.OrdinalIgnoreCase)
				.ToList();

			foreach (var name in missing)
				context.AddFailure(name, "Required.");
		});
	}
}