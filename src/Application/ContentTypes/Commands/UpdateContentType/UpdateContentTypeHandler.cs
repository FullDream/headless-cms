using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Commands.UpdateContentType;

public class UpdateContentTypeHandler(IContentTypeRepository repository)
	: IRequestHandler<UpdateContentTypeCommand, ContentTypeDto>
{
	public async Task<ContentTypeDto> Handle(UpdateContentTypeCommand request, CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.Id, cancellationToken);

		if (contentType is null)
			throw new InvalidOperationException($"ContentType with id {request.Id}  is not found");

		if (request.Name is { } name && name != contentType.Name)
		{
			var duplicate = await repository.FindByNameAsync(name, cancellationToken);

			if (duplicate is not null)
				throw new InvalidOperationException($"Content Type '{name}' is already exists");

			contentType.Rename(name);
		}

		if (request.Kind is { } kind && kind != contentType.Kind)
			contentType.ChangeKind(kind);

		var updated = await repository.UpdateAsync(contentType, cancellationToken);

		return updated.ToDto();
	}
}