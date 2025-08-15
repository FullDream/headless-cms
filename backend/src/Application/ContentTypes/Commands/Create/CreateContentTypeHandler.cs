using Application.Abstractions;
using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;
using SharedKernel.Result;

namespace Application.ContentTypes.Commands.Create;

public class CreateContentTypeHandler(IContentTypeRepository repository, IContentTypeSchemaManager schemaManager)
	: IRequestHandler<CreateContentTypeCommand, Result<ContentTypeDto>>
{
	public async Task<Result<ContentTypeDto>> Handle(CreateContentTypeCommand request,
		CancellationToken cancellationToken)
	{
		var contentType = new ContentType(request.Name, request.Kind);

		var fieldsResult = contentType.AddFields(request.Fields.Select(f => (f.Name, f.Label, f.Type, f.IsRequired)));

		if (fieldsResult.IsFailure) return fieldsResult.Errors;

		repository.Add(contentType);

		await repository.SaveChangesAsync(cancellationToken);
		await schemaManager.EnsureStructureCreatedAsync(contentType, cancellationToken);

		return contentType.ToDto();
	}
}