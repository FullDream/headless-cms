using SharedKernel.Result;

namespace Iam.Domain.AccessResource;

public sealed class AccessResource
{
	private readonly List<ResourceCapability> capabilities;

	private AccessResource()
	{
	}

	private AccessResource(
		Guid id,
		string key,
		string name,
		Guid? parentId,
		IEnumerable<ResourceCapability> capabilities)
	{
		Id = id;
		Key = key;
		Name = name;
		ParentId = parentId;
		this.capabilities = [.. capabilities];
	}

	public Guid Id { get; }

	public string Key { get; }

	public string Name { get; }

	public Guid? ParentId { get; }

	public IReadOnlyCollection<ResourceCapability> Capabilities => capabilities;

	public static Result<AccessResource> Create(
		Guid id,
		string key,
		string name,
		IEnumerable<ResourceCapability> capabilities,
		Guid? parentId = null)
	{
		var capabilityList = capabilities.ToList();

		if (capabilityList.Select(x => x.Action).Distinct().Count() != capabilityList.Count)
			return AccessResourceErrors.DuplicateCapability;

		return new AccessResource(id, key, name, parentId, capabilityList);
	}

	public static Result<AccessResource> Create(
		string key,
		string name,
		IEnumerable<ResourceCapability> capabilities,
		Guid? parentId = null) =>
		Create(Guid.NewGuid(), key, name, capabilities, parentId);

	public bool Supports(PermissionAction action, AccessScope scope) =>
		capabilities.Any(x => x.Action == action && x.Supports(scope));
}
