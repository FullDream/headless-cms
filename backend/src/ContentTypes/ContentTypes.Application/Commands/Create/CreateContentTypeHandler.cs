using Application.Abstractions;
using ContentTypes.Application.Dtos;
using ContentTypes.Application.Mappers;
using ContentTypes.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentTypes.Application.Commands.Create;

public class CreateContentTypeHandler(
	IContentTypeRepository repository,
	IContentTypeExistenceChecker checker)
	: IRequestHandler<CreateContentTypeCommand, Result<ContentTypeDto>>
{
	public async Task<Result<ContentTypeDto>> Handle(CreateContentTypeCommand request,
		CancellationToken cancellationToken)
	{
		var isExist = await checker.ExistsByNameAsync(request.Name, cancellationToken);

		if (isExist) return ContentTypeErrors.AlreadyExist(request.Name);

		var contentType = ContentType.Create(request.Name, request.Kind);

		var fieldsResult =
			contentType.AddFields(request.Fields.Select(f => (f.Name, f.Label, f.Type, f.IsRequired)).ToArray());

		if (fieldsResult.IsFailure) return fieldsResult.Errors;

		repository.Add(contentType);

		await repository.SaveChangesAsync(cancellationToken);
		// await schemaManager.EnsureStructureCreatedAsync(contentType, cancellationToken);

		return contentType.ToDto();
	}
}