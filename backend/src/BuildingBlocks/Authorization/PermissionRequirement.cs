namespace BuildingBlocks.Authorization;

public readonly record struct PermissionRequirement(string Resource, string Action);
