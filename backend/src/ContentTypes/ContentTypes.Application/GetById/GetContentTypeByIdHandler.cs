using ContentTypes.Application.Common.ContentType;
using ContentTypes.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentTypes.Application.GetById;

internal sealed class GetContentTypeByIdHandler(IContentTypeRepository repository)
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