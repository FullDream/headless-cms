using Application.Abstractions;
using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Commands.Create;

public class CreateContentTypeHandler(IContentTypeRepository repository, IContentTypeSchemaManager schemaManager)
	: IRequestHandler<CreateContentTypeCommand, ContentTypeDto>
{
	public async Task<ContentTypeDto> Handle(CreateContentTypeCommand request, CancellationToken cancellationToken)
	{
		var exists = await repository.FindByNameAsync(request.Name, cancellationToken);

		if (exists is not null)
			throw new InvalidOperationException($"ContentType '{request.Name}' already exists.");

		var contentType = new ContentType(Guid.NewGuid(), request.Name, request.Kind);

		contentType.AddFields(request.Fields.Select(f => (f.Name, f.Label, f.Type, f.IsRequired)));

		await schemaManager.EnsureStructureCreatedAsync(contentType, cancellationToken);

		repository.Add(contentType);
		await repository.SaveChangesAsync(cancellationToken);

		return contentType.ToDto();
	}
}