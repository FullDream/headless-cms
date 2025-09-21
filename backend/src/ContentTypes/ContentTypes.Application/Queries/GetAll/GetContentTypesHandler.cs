using ContentTypes.Application.Dtos;
using ContentTypes.Application.Mappers;
using ContentTypes.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentTypes.Application.Queries.GetAll;

public class GetContentTypesHandler(IContentTypeRepository repository)
	: IRequestHandler<GetContentTypesQuery, Result<IEnumerable<ContentTypeDto>>>
{
	public async Task<Result<IEnumerable<ContentTypeDto>>> Handle(GetContentTypesQuery request,
		CancellationToken cancellationToken)
	{
		var contentTypes = await repository.FindManyAsync(request.Kind, cancellationToken);

		return Result<IEnumerable<ContentTypeDto>>.Success(contentTypes.Select(ct => ct.ToDto()));
	}
}