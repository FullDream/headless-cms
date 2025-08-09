using Application.Abstractions;
using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Commands.RemoveField;

public class RemoveFieldFromContentTypeHandler(
	IContentTypeRepository repository,
	IContentTypeSchemaManager schemaManager)
	: IRequestHandler<RemoveFieldFromContentTypeCommand, ContentFieldDto?>
{
	public async Task<ContentFieldDto?> Handle(RemoveFieldFromContentTypeCommand request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.ContentTypeId, cancellationToken);

		if (contentType is null) throw new InvalidOperationException($"ContentType is not found");

		var field = contentType.RemoveField(request.ContentFieldId);

		if (field is null) return null;

		await repository.SaveChangesAsync(cancellationToken);

		await schemaManager.RemoveFieldFromStructureAsync(contentType, field, cancellationToken);

		return field.ToDto();
	}
}