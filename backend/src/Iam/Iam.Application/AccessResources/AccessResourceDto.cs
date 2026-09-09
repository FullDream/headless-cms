namespace Iam.Application.AccessResources;

public sealed record AccessResourceDto(
	Guid Id,
	string Key,
	string Name,
	IReadOnlyCollection<ResourceCapabilityDto> Capabilities,
	IReadOnlyCollection<AccessResourceDto> Children);
