using SharedKernel.Result;

namespace Iam.Domain.AccessResource;

public static class ResourceCapabilityErrors
{
	public static readonly Error ScopeRequired = new(
		"ResourceCapability.ScopeRequired",
		"At least one access scope must be specified.",
		Type: ErrorType.BusinessRule);

	public static readonly Error DuplicateScope = new(
		"ResourceCapability.DuplicateScope",
		"Access scopes must be unique.",
		Type: ErrorType.BusinessRule);
}
