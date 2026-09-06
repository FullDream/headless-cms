namespace BuildingBlocks.Authorization;

public interface IRequirePermission
{
	PermissionRequirement Permission { get; }
}
