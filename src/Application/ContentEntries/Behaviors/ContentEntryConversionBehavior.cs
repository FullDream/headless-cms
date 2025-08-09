using Application.Abstractions;
using Application.ContentEntries.Commands;
using Application.ContentEntries.Conversion;
using MediatR;

namespace Application.ContentEntries.Behaviors;

public sealed class ContentEntryConversionBehavior<TRequest, TResponse>(
	IContentTypeFieldsProvider schema) : IPipelineBehavior<TRequest, TResponse>
	where TRequest : ContentEntryCommandBase
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
	{
		var snap = await schema.FindByNameAsync(request.ContentTypeName, ct);

		if (snap is null)
			throw new KeyNotFoundException(
				$"Content type '{request.ContentTypeName}' not found.");

		var convertedFields = snap.Fields
			.Where(fieldDef => request.Fields.ContainsKey(fieldDef.Key))
			.ToDictionary(
				fieldDef => fieldDef.Key,
				fieldDef => JsonFieldValueConverter.Convert(request.Fields[fieldDef.Key], fieldDef.Value)
			);

		request.SetConvertedFields(convertedFields);

		return await next(ct);
	}
}