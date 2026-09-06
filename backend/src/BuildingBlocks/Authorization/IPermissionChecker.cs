namespace BuildingBlocks.Authorization;

public interface IPermissionChecker
{
	Task<PermissionAccess> CheckAsync(PermissionRequirement permission, CancellationToken cancellationToken = default);
}
