using Application.Abstractions;
using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;
using SharedKernel.Result;

namespace Application.ContentTypes.Commands.AddField;

public class AddFieldToContentTypeHandler(IContentTypeRepository repository, IContentTypeSchemaManager schemaManager)
	: IRequestHandler<AddFieldToContentTypeCommand, Result<ContentFieldDto>>
{
	public async Task<Result<ContentFieldDto>> Handle(AddFieldToContentTypeCommand request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.ContentTypeId, cancellationToken);

		if (contentType is null) return ContentTypeErrors.NotFound(nameof(request.ContentTypeId));

		var fieldRequest = request.Field;
		var result = contentType.AddField(fieldRequest.Name, fieldRequest.Label, fieldRequest.Type,
			fieldRequest.IsRequired);

		if (result.IsFailure) return result.Errors;

		repository.AddField(result.Value);

		await repository.SaveChangesAsync(cancellationToken);

		await schemaManager.AddFieldToStructureAsync(contentType, result.Value, cancellationToken);

		return result.Value.ToDto();
	}
}