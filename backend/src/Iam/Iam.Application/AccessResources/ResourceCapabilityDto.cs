using Iam.Domain;

namespace Iam.Application.AccessResources;

public sealed record ResourceCapabilityDto(PermissionAction Action, IReadOnlyCollection<AccessScope> AvailableScopes);
