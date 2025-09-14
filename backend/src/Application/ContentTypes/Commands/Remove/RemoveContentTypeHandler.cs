using Application.Abstractions;
using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;
using SharedKernel.Result;

namespace Application.ContentTypes.Commands.Remove;

public class RemoveContentTypeHandler(IContentTypeRepository repository, IContentTypeSchemaManager schemaManager)
	: IRequestHandler<RemoveContentTypeCommand, Result<ContentTypeDto>>
{
	public async Task<Result<ContentTypeDto>> Handle(RemoveContentTypeCommand request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.Id, cancellationToken: cancellationToken);

		if (contentType is null) return ContentTypeErrors.NotFound(nameof(request.Id));

		await schemaManager.RemoveStructureAsync(contentType, cancellationToken);

		contentType.Remove();
		repository.Remove(contentType);
		await repository.SaveChangesAsync(cancellationToken);

		return contentType.ToDto();
	}
}