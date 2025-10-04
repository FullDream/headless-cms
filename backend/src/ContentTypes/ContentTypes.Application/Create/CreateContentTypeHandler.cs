using BuildingBlocks;
using ContentTypes.Application.AddField;
using ContentTypes.Application.Common.ContentType;
using ContentTypes.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentTypes.Application.Create;

internal sealed class CreateContentTypeHandler(
	IContentTypeRepository repository,
	IContentTypeExistenceChecker checker)
	: IRequestHandler<CreateContentTypeCommand, Result<ContentTypeDto>>
{
	public async Task<Result<ContentTypeDto>> Handle(CreateContentTypeCommand request,
		CancellationToken cancellationToken)
	{
		var isExist = await checker.ExistsByNameAsync(request.Name, cancellationToken);

		if (isExist) return ContentTypeErrors.AlreadyExist(request.Name);

		var contentType = ContentType.Create(request.Name,
			request.Kind,
			request.Fields.Select(field => field.ToDefinition()).ToArray());


		if (contentType.IsFailure) return contentType.Errors;

		repository.Add(contentType.Value);

		await repository.SaveChangesAsync(cancellationToken);

		return contentType.Value.ToDto();
	}
}