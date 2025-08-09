using Application.Abstractions;
using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Commands.Update;

public class UpdateContentTypeHandler(IContentTypeRepository repository, IContentTypeSchemaManager schemaManager)
	: IRequestHandler<UpdateContentTypeCommand, ContentTypeDto>
{
	public async Task<ContentTypeDto> Handle(UpdateContentTypeCommand request, CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.Id, cancellationToken);

		if (contentType is null)
			throw new InvalidOperationException($"ContentType with id {request.Id}  is not found");

		var oldName = contentType.Name;
		if (request.Name is { } name && name != oldName)
		{
			var duplicate = await repository.FindByNameAsync(name, cancellationToken);

			if (duplicate is not null)
				throw new InvalidOperationException($"Content Type '{name}' is already exists");

			contentType.Rename(name);

			await schemaManager.RenameStructureAsync(oldName, name, cancellationToken);
		}

		if (request.Kind is { } kind && kind != contentType.Kind)
			contentType.ChangeKind(kind);

		repository.Update(contentType);

		await repository.SaveChangesAsync(cancellationToken);


		return contentType.ToDto();
	}
}