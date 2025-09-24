using ContentEntries.Application.Common;
using ContentEntries.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentEntries.Application.Create;

internal sealed class CreateContentEntryHandler(IContentEntryRepository repository)
	: IRequestHandler<CreateContentEntryCommand, Result<IReadOnlyDictionary<string, object?>>>
{
	public async Task<Result<IReadOnlyDictionary<string, object?>>> Handle(CreateContentEntryCommand request,
		CancellationToken cancellationToken)
	{
		var contentEntry = ContentEntry.Create(request.ConvertedFields);

		await repository.AddAsync(request.ContentTypeName, contentEntry, cancellationToken);

		return Result<IReadOnlyDictionary<string, object?>>.Success(contentEntry.ToDto());
	}
}