using SharedKernel.Result;

namespace Iam.Domain.AccessResource;

public static class AccessResourceErrors
{
	public static readonly Error DuplicateCapability = new(
		"AccessResource.DuplicateCapability",
		"Resource capabilities must have unique actions.",
		Type: ErrorType.BusinessRule);
}
