using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Commands.DeleteContentType;

public class DeleteContentTypeHandler(IContentTypeRepository repository)
	: IRequestHandler<DeleteContentTypeCommand, ContentTypeDto?>
{
	public async Task<ContentTypeDto?> Handle(DeleteContentTypeCommand request, CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.Id, cancellationToken: cancellationToken);

		if (contentType is null) return null;

		repository.Remove(contentType);
		await repository.SaveChangesAsync(cancellationToken);

		return contentType.ToDto();
	}
}