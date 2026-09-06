namespace Iam.Application.AccessResources;

public sealed record AccessResourceDto(
	Guid Id,
	string Key,
	string Name,
	Guid? ParentId,
	IReadOnlyCollection<ResourceCapabilityDto> Capabilities);
