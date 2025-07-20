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
		var deleted = await repository.DeleteAsync(request.Id, cancellationToken);

		return deleted?.ToDto();
	}
}