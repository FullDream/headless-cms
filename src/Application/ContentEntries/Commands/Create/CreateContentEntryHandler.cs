using Application.Abstractions;
using Application.ContentEntries.Mappers;
using Core.ContentEntries;
using MediatR;

namespace Application.ContentEntries.Commands.Create;

internal sealed class CreateContentEntryHandler(
	IContentEntryRepository repository,
	IContentTypeFieldsProvider contentTypeFieldsProvider)
	: IRequestHandler<CreateContentEntryCommand, IReadOnlyDictionary<string, object?>>
{
	public async Task<IReadOnlyDictionary<string, object?>> Handle(CreateContentEntryCommand request,
		CancellationToken cancellationToken)
	{
		var contentEntry = ContentEntry.Create(request.ConvertedFields);

		await repository.AddAsync(request.ContentTypeName, contentEntry, cancellationToken);


		return contentEntry.ToDto();
	}
}