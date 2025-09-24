using BuildingBlocks.Messaging;
using ContentTypes.Application.Common.ContentField;

namespace ContentTypes.Application.RemoveField;

public sealed record RemoveFieldFromContentTypeCommand(Guid ContentTypeId, Guid ContentFieldId)
	: ICommand<ContentFieldDto>;