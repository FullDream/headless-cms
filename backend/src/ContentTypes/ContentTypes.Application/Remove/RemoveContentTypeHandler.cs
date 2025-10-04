using ContentTypes.Application.Common.ContentType;
using ContentTypes.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentTypes.Application.Remove;

internal sealed class RemoveContentTypeHandler(IContentTypeRepository repository)
	: IRequestHandler<RemoveContentTypeCommand, Result<ContentTypeDto>>
{
	public async Task<Result<ContentTypeDto>> Handle(RemoveContentTypeCommand request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.Id, cancellationToken: cancellationToken);

		if (contentType is null) return ContentTypeErrors.NotFound(nameof(request.Id));

		contentType.Remove();
		repository.Remove(contentType);
		await repository.SaveChangesAsync(cancellationToken);

		return contentType.ToDto();
	}
}