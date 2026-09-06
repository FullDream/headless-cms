using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;
using ContentTypes.Application.Authorization;
using ContentTypes.Application.Common.ContentField;
using ContentTypes.Application.Update;

namespace ContentTypes.Application.UpdateField;

public sealed record UpdateFieldInContentTypeCommand(
	Guid ContentTypeId,
	Guid ContentFieldId,
	UpdateContentFieldDto UpdateDto) : ICommand<ContentFieldDto>, IRequirePermission
{
	public PermissionRequirement Permission => ContentTypePermissions.Update;
}
