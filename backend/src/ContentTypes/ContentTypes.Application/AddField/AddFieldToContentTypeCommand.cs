using BuildingBlocks.Messaging;
using ContentTypes.Application.Common.ContentField;
using ContentTypes.Application.Create;

namespace ContentTypes.Application.AddField;

public sealed record AddFieldToContentTypeCommand(Guid ContentTypeId, CreateContentFieldDto Field)
	: ICommand<ContentFieldDto>;