using ContentTypes.Application.Common.ContentType;
using ContentTypes.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentTypes.Application.Update;

internal sealed class UpdateContentTypeHandler(IContentTypeRepository repository)
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
		}

		if (request.Kind is { } kind && kind != contentType.Kind)
			contentType.ChangeKind(kind);

		repository.Update(contentType);

		await repository.SaveChangesAsync(cancellationToken);


		return contentType.ToDto();
	}
}