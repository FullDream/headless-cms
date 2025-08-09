using Application.Abstractions;
using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Commands.AddField;

public class AddFieldToContentTypeHandler(IContentTypeRepository repository, IContentTypeSchemaManager schemaManager)
	: IRequestHandler<AddFieldToContentTypeCommand, ContentFieldDto>
{
	public async Task<ContentFieldDto> Handle(AddFieldToContentTypeCommand request, CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.ContentTypeId, cancellationToken);

		if (contentType is null) throw new InvalidOperationException($"ContentType  is not found");

		var fieldRequest = request.Field;
		var duplicate = contentType.Fields.FirstOrDefault(f => f.Name == fieldRequest.Name);

		if (duplicate is not null)
			throw new InvalidOperationException($"Field with name {request.Field.Name} already exists");

		var field = contentType.AddField(fieldRequest.Name, fieldRequest.Label, fieldRequest.Type,
			fieldRequest.IsRequired);

		repository.AddField(field);

		await repository.SaveChangesAsync(cancellationToken);

		await schemaManager.AddFieldToStructureAsync(contentType, field, cancellationToken);

		return field.ToDto();
	}
}