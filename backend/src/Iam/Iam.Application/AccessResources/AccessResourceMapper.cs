using Iam.Domain.AccessResource;

namespace Iam.Application.AccessResources;

internal static class AccessResourceMapper
{
	internal static IReadOnlyCollection<AccessResourceDto> ToTreeDto(this IReadOnlyCollection<AccessResource> resources)
	{
		var childrenByParentId = resources
			.Where(resource => resource.ParentId is not null)
			.ToLookup(resource => resource.ParentId!.Value);

		AccessResourceDto Map(AccessResource resource) =>
			resource.ToDto(
				childrenByParentId[resource.Id]
					.Select(Map)
					.ToArray());

		return resources
			.Where(resource => resource.ParentId is null)
			.Select(Map)
			.ToArray();
	}

	private static AccessResourceDto ToDto(
		this AccessResource resource,
		IReadOnlyCollection<AccessResourceDto> children) =>
		new(
			resource.Id,
			resource.Key,
			resource.Name,
			resource.Capabilities
				.Select(capability => capability.ToDto())
				.ToArray(),
			children);

	private static ResourceCapabilityDto ToDto(this ResourceCapability capability) =>
		new(
			capability.Action,
			capability.AvailableScopes.ToArray());
}
