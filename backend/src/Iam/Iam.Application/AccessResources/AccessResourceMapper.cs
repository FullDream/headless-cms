using Iam.Domain.AccessResource;

namespace Iam.Application.AccessResources;

internal static class AccessResourceMapper
{
	internal static AccessResourceDto ToDto(this AccessResource resource) =>
		new(
			resource.Id,
			resource.Key,
			resource.Name,
			resource.ParentId,
			resource.Capabilities.Select(capability => capability.ToDto()).ToArray());

	private static ResourceCapabilityDto ToDto(this ResourceCapability capability) =>
		new(capability.Action, capability.AvailableScopes.ToArray());
}
