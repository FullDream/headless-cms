using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;
using SharedKernel.Result;

namespace Application.ContentTypes.Queries.GetByName;

public class GetContentTypeByNameHandler(IContentTypeRepository repository)
	: IRequestHandler<GetContentTypeByNameQuery, Result<ContentTypeDto>>
{
	public async Task<Result<ContentTypeDto>> Handle(GetContentTypeByNameQuery request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByNameAsync(request.Name, cancellationToken);

		if (contentType == null) return ContentTypeErrors.NotFound(nameof(request.Name));

		return contentType.ToDto();
	}
}