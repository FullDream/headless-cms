using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Queries.GetById;

public class GetContentTypeByIdHandler(IContentTypeRepository repository)
	: IRequestHandler<GetContentTypeByIdQuery, ContentTypeDto?>
{
	public async Task<ContentTypeDto?> Handle(GetContentTypeByIdQuery request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.Id, cancellationToken);

		return contentType?.ToDto();
	}
}