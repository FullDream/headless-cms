using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;
using ContentTypes.Application.Authorization;
using ContentTypes.Application.Common.ContentField;

namespace ContentTypes.Application.AddField;

public sealed record AddFieldToContentTypeCommand(Guid ContentTypeId, CreateContentFieldDto Field)
	: ICommand<ContentFieldDto>, IRequirePermission
{
	public PermissionRequirement Permission => ContentTypePermissions.Update;
}
