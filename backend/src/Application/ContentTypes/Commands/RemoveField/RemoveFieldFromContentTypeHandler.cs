using Application.Abstractions;
using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;
using SharedKernel.Result;

namespace Application.ContentTypes.Commands.RemoveField;

public class RemoveFieldFromContentTypeHandler(
	IContentTypeRepository repository,
	IContentTypeSchemaManager schemaManager)
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

		await schemaManager.RemoveFieldFromStructureAsync(contentType, fieldResult.Value, cancellationToken);

		return fieldResult.Value.ToDto();
	}
}