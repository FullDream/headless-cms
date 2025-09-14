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
		var result = contentType.AddFields((fieldRequest.Name, fieldRequest.Label, fieldRequest.Type,
			fieldRequest.IsRequired));

		if (result.IsFailure) return result.Errors;

		var field = result.Value.First();

		repository.AddField(result.Value.First());

		await repository.SaveChangesAsync(cancellationToken);

		await schemaManager.AddFieldToStructureAsync(contentType, field, cancellationToken);

		return field.ToDto();
	}
}