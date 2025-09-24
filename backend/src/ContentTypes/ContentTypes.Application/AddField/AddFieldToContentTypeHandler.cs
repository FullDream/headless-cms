using ContentTypes.Application.Common.ContentField;
using ContentTypes.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentTypes.Application.AddField;

internal sealed class AddFieldToContentTypeHandler(IContentTypeRepository repository)
	: IRequestHandler<AddFieldToContentTypeCommand, Result<ContentFieldDto>>
{
	public async Task<Result<ContentFieldDto>> Handle(AddFieldToContentTypeCommand request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.ContentTypeId, cancellationToken);

		if (contentType is null) return ContentTypeErrors.NotFound(nameof(request.ContentTypeId));

		var result = contentType.AddField(request.Field.ToDefinition());

		if (result.IsFailure) return result.Errors;

		var field = result.Value;

		repository.AddField(field);

		await repository.SaveChangesAsync(cancellationToken);

		// await schemaManager.AddFieldToStructureAsync(contentType, field, cancellationToken);

		return field.ToDto();
	}
}