using SharedKernel.Result;

namespace Iam.Domain.AccessResource;

public sealed class ResourceCapability
{
	private readonly HashSet<AccessScope> availableScopes;

	private ResourceCapability()
	{
	}

	private ResourceCapability(PermissionAction action, AccessScope[] availableScopes)
	{
		Action = action;
		this.availableScopes = [.. availableScopes];
	}

	public PermissionAction Action { get; }

	public IReadOnlySet<AccessScope> AvailableScopes => availableScopes;

	public static Result<ResourceCapability> Create(PermissionAction action, params AccessScope[] availableScopes)
	{
		if (availableScopes.Length == 0)
			return ResourceCapabilityErrors.ScopeRequired;

		if (availableScopes.Distinct().Count() != availableScopes.Length)
			return ResourceCapabilityErrors.DuplicateScope;

		return new ResourceCapability(action, availableScopes);
	}

	public bool Supports(AccessScope scope) => availableScopes.Contains(scope);
}
