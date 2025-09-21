using ContentTypes.Application.Dtos;
using ContentTypes.Application.Mappers;
using ContentTypes.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentTypes.Application.Commands.Remove;

public class RemoveContentTypeHandler(IContentTypeRepository repository)
	: IRequestHandler<RemoveContentTypeCommand, Result<ContentTypeDto>>
{
	public async Task<Result<ContentTypeDto>> Handle(RemoveContentTypeCommand request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.Id, cancellationToken: cancellationToken);

		if (contentType is null) return ContentTypeErrors.NotFound(nameof(request.Id));

		// await schemaManager.RemoveStructureAsync(contentType, cancellationToken);

		contentType.Remove();
		repository.Remove(contentType);
		await repository.SaveChangesAsync(cancellationToken);

		return contentType.ToDto();
	}
}