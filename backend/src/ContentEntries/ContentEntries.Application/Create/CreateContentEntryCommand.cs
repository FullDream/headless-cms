using System.Text.Json;
using BuildingBlocks.Messaging;
using ContentEntries.Application.Common;

namespace ContentEntries.Application.Create;

public sealed record CreateContentEntryCommand(
	string ContentTypeName,
	Dictionary<string, JsonElement> Fields
) : ContentEntryCommandBase(ContentTypeName, Fields), ICommand<IReadOnlyDictionary<string, object?>>;