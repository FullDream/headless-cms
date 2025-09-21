using ContentTypes.Application.Dtos;
using ContentTypes.Application.Mappers;
using ContentTypes.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentTypes.Application.Queries.GetById;

public class GetContentTypeByIdHandler(IContentTypeRepository repository)
	: IRequestHandler<GetContentTypeByIdQuery, Result<ContentTypeDto>>
{
	public async Task<Result<ContentTypeDto>> Handle(GetContentTypeByIdQuery request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.Id, cancellationToken);

		if (contentType == null) return ContentTypeErrors.NotFound(nameof(request.Id));

		return contentType.ToDto();
	}
}