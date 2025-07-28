using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Commands.RemoveContentType;

public class RemoveContentTypeHandler(IContentTypeRepository repository)
	: IRequestHandler<RemoveContentTypeCommand, ContentTypeDto?>
{
	public async Task<ContentTypeDto?> Handle(RemoveContentTypeCommand request, CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.Id, cancellationToken: cancellationToken);

		if (contentType is null) return null;

		repository.Remove(contentType);
		await repository.SaveChangesAsync(cancellationToken);

		return contentType.ToDto();
	}
}