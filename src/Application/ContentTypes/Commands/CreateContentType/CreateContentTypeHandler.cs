using Application.ContentTypes.Dtos;
using Application.ContentTypes.Mappers;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Commands.CreateContentType;

public class CreateContentTypeHandler(IContentTypeRepository repository)
	: IRequestHandler<CreateContentTypeCommand, ContentTypeDto>
{
	public async Task<ContentTypeDto> Handle(CreateContentTypeCommand request, CancellationToken cancellationToken)
	{
		var exists = await repository.FindByNameAsync(request.Name, cancellationToken);

		if (exists is not null)
			throw new InvalidOperationException($"ContentType '{request.Name}' already exists.");

		var contentType = new ContentType(Guid.NewGuid(), request.Name, request.Kind);
		var created = await repository.AddAsync(contentType, cancellationToken);

		return created.ToDto();
	}
}