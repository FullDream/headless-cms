using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Queries.GetContentTypeByName;

public class GetContentTypeByNameHandler(IContentTypeRepository repository)
	: IRequestHandler<GetContentTypeByNameQuery, ContentTypeDto?>
{
	public async Task<ContentTypeDto?> Handle(GetContentTypeByNameQuery request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByNameAsync(request.Name, cancellationToken);

		return contentType?.ToDto();
	}
}