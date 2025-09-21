using ContentTypes.Application.Dtos;
using ContentTypes.Application.Mappers;
using ContentTypes.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentTypes.Application.Queries.GetByName;

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