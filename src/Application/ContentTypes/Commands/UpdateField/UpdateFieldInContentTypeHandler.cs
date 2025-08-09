using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Commands.UpdateField;

public class UpdateFieldInContentTypeHandler(IContentTypeRepository repository)
	: IRequestHandler<UpdateFieldInContentTypeCommand, ContentFieldDto?>
{
	public async Task<ContentFieldDto?> Handle(UpdateFieldInContentTypeCommand request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.ContentTypeId, cancellationToken);

		if (contentType is null) throw new InvalidOperationException($"ContentType is not found");


		var field = contentType.UpdateField(request.ContentFieldId,
			new ContentFieldPatch(
				Name: request.UpdateDto.Name,
				Label: request.UpdateDto.Label,
				Type: request.UpdateDto.Type,
				IsRequired: request.UpdateDto.IsRequired
			)
		);

		await repository.SaveChangesAsync(cancellationToken);

		return field.ToDto();
	}
}