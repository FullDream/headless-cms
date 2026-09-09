using Iam.Domain.AccessResource;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.AccessResources.GetAccessResources;

internal sealed class GetAccessResourcesQueryHandler(IAccessResourceRepository resourceRepository)
	: IRequestHandler<GetAccessResourcesQuery, Result<IReadOnlyCollection<AccessResourceDto>>>
{
	public async Task<Result<IReadOnlyCollection<AccessResourceDto>>> Handle(
		GetAccessResourcesQuery request,
		CancellationToken cancellationToken)
	{
		var resources = await resourceRepository.FindManyAsync(cancellationToken);

		return Result<IReadOnlyCollection<AccessResourceDto>>.Success(resources.ToTreeDto());
	}
}
