using ContentTypes.Application.Common.ContentField;
using ContentTypes.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentTypes.Application.RemoveField;

internal sealed class RemoveFieldFromContentTypeHandler(
	IContentTypeRepository repository)
	: IRequestHandler<RemoveFieldFromContentTypeCommand, Result<ContentFieldDto>>
{
	public async Task<Result<ContentFieldDto>> Handle(RemoveFieldFromContentTypeCommand request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.ContentTypeId, cancellationToken);

		if (contentType is null) return ContentTypeErrors.NotFound(nameof(request.ContentTypeId));

		var fieldResult = contentType.RemoveField(request.ContentFieldId);

		if (fieldResult.IsFailure) return fieldResult.Errors;

		await repository.SaveChangesAsync(cancellationToken);

		// await schemaManager.RemoveFieldFromStructureAsync(contentType, fieldResult.Value, cancellationToken);

		return fieldResult.Value.ToDto();
	}
}