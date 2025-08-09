using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Queries.GetAll;

public class GetContentTypesHandler(IContentTypeRepository repository)
	: IRequestHandler<GetContentTypesQuery, IEnumerable<ContentTypeDto>>
{
	public async Task<IEnumerable<ContentTypeDto>> Handle(GetContentTypesQuery request,
		CancellationToken cancellationToken)
	{
		var contentTypes = await repository.FindManyAsync(request.Kind, cancellationToken);

		return contentTypes.Select(ct => ct.ToDto());
	}
}