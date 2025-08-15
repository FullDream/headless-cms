using Application.Abstractions;
using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;
using SharedKernel.Result;

namespace Application.ContentTypes.Commands.Update;

public class UpdateContentTypeHandler(IContentTypeRepository repository, IContentTypeSchemaManager schemaManager)
	: IRequestHandler<UpdateContentTypeCommand, Result<ContentTypeDto>>
{
	public async Task<Result<ContentTypeDto>> Handle(UpdateContentTypeCommand request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.Id, cancellationToken);

		if (contentType is null) return ContentTypeErrors.NotFound(nameof(request.Id));

		var oldName = contentType.Name;
		if (request.Name is { } name && name != oldName)
		{
			var duplicate = await repository.FindByNameAsync(name, cancellationToken);

			if (duplicate is not null)
				return ContentTypeErrors.AlreadyExist(name);

			contentType.Rename(name);

			await schemaManager.RenameStructureAsync(oldName, name, cancellationToken);
		}

		if (request.Kind is { } kind && kind != contentType.Kind)
			contentType.ChangeKind(kind);

		repository.Update(contentType);

		await repository.SaveChangesAsync(cancellationToken);


		return contentType.ToDto();
	}
}