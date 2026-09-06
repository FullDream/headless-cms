using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;
using ContentTypes.Application.Authorization;
using ContentTypes.Application.Common.ContentField;

namespace ContentTypes.Application.RemoveField;

public sealed record RemoveFieldFromContentTypeCommand(Guid ContentTypeId, Guid ContentFieldId)
	: ICommand<ContentFieldDto>, IRequirePermission
{
	public PermissionRequirement Permission => ContentTypePermissions.Update;
}
